using BlazorWASMCustomAuth.Database;
using BlazorWASMCustomAuth.PagingSortingFiltering;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Validations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public class SecurityService
    {
        private DatabaseManager dm;
        private readonly TokenSettingsModel _tokenSettings;

        public SecurityService(IOptions<TokenSettingsModel> tokenSettings)
        {
            _tokenSettings = tokenSettings.Value;
            dm = new DatabaseManager("Server=.;Database=SecurityTablesTest;Trusted_Connection=True");
        }
        public TokenModel UserLogin(UserLoginModel model)
        {
            if (model.Username.IsNullOrEmpty() || model.Password.IsNullOrEmpty())
                return null;

            bool IsUserAuthenticated = AuthenticateUser(model.Username, model.Password);

            if (!IsUserAuthenticated)
                return null;

            var token = GenerateAuthenticationToken(model.Username);
            UpdateRefreshTokenInDB(model.Username, token.Token, token.RefreshToken);
            return token;
        }
        private bool AuthenticateUser(string username, string password, string authenticationProviderName = "")
        {
            if (username.IsNullOrEmpty() || password.IsNullOrEmpty())
                return false;

            if (authenticationProviderName.IsNullOrEmpty())
                authenticationProviderName = GetDefaultAuthenticationProviderName(username);

            bool IsUserAuthenticated = false;
            switch (authenticationProviderName)
            {
                case "Active Directory":
                    IsUserAuthenticated = AuthenticateUserWithDomain(username, password);
                    break;
                default: //Local
                    IsUserAuthenticated = AuthenticateUserWithLocalPassword(username, password);
                    break;
            }
            return IsUserAuthenticated;
        }
        private string GetDefaultAuthenticationProviderName(string username)
        {
            string result;
            try
            {
                var dbresult = dm.ExecScalarSP("sp_UserGetDefaultAuthenticationProveder", "Username", username);
                if (dbresult.HasError || dbresult.Result == null)
                    return null;

                result = dbresult.Result.ToString();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private UserModel UserGet(string username)
        {
            UserModel user = new UserModel();

            var UserDt = dm.ExecDataTableSP("sp_UserList", "Username", username);
            foreach (DataRow dr in UserDt.Rows)
            {
                user.Id = int.Parse(dr["Id"].ToString() ?? "");
                user.Name = dr["Name"].ToString();
                user.Username = dr["Username"].ToString();
                user.Email = dr["Email"].ToString();
                user.Permissions = GetUserPermissions(username);
            }

            return user;
        }
        private string GetMappedUsername(string username)
        {
            string result;
            try
            {
                var dbresult = dm.ExecScalarSP("sp_UserGetMappedUsername", "AuthenticationProviderName", "Local", "Username", username);
                if (dbresult.HasError || dbresult.Result == null)
                    return null;

                result = dbresult.Result.ToString();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private string GetPasswordHashed(string username)
        {
            string result;
            try
            {
                var dbresult = dm.ExecScalarSP("sp_UserGetPasswordHashed", "Username", username);
                if (dbresult.HasError || dbresult.Result == null)
                    return null;

                result = dbresult.Result.ToString();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private string GetPasswordSalt(string username)
        {
            string result;
            try
            {
                var dbresult = dm.ExecScalarSP("sp_UserGetPasswordSalt", "Username", username);
                if (dbresult.HasError || dbresult.Result == null)
                    return null;

                result = dbresult.Result.ToString();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private bool AuthenticateUserWithLocalPassword(string username, string password)
        {
            if (username.IsNullOrEmpty() || password.IsNullOrEmpty())
                return false;

            string mappedUserName = GetMappedUsername(username);
            if (mappedUserName == null)
                mappedUserName = username;

            string HashedPassword = GetPasswordHashed(username);
            if (HashedPassword == null)
                return false;

            string SaltPassword = GetPasswordSalt(username);
            if (SaltPassword == null)
                return false;

            byte[] bytesalt = Convert.FromHexString(SaltPassword);
            const int keySize = 64;
            const int iterations = 350000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, bytesalt, iterations, hashAlgorithm, keySize);
            return hashToCompare.SequenceEqual(Convert.FromHexString(HashedPassword));
        }
        private bool AuthenticateUserWithDomain(string username, string password)
        {
            if (username.IsNullOrEmpty() || password.IsNullOrEmpty())
                return false;

            string mappedUserName = dm.ExecScalarSP("sp_UserGetMappedUsername", "AuthenticationProviderName", "Active Directory", "Username", username).Result.ToString();
            if (mappedUserName == null)
                mappedUserName = username;

#pragma warning disable IDE0063 // Use simple 'using' statement
#pragma warning disable CA1416 // Validate platform compatibility
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "DOMAIN", "USERNAME", "PASSWORD"))
            {
                // validate the credentials
                bool isValid = pc.ValidateCredentials(mappedUserName, password);
                return isValid;
            }
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore IDE0063 // Use simple 'using' statement

        }
        private TokenModel GenerateAuthenticationToken(string username)
        {
            if (username.IsNullOrEmpty())
                return null;

            var user = UserGet(username);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key ?? ""));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new List<Claim>();
            userClaims.Add(new Claim(ClaimTypes.Email, user.Email ?? ""));
            userClaims.Add(new Claim("UserId", user.Id.ToString()));
            userClaims.Add(new Claim("UserName", user.Username ?? ""));

            foreach (var permission in user.Permissions)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, permission));
            }

            var newJwtToken = new JwtSecurityToken(
                    issuer: _tokenSettings.Issuer,
                    audience: _tokenSettings.Audience,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: credentials,
                    claims: userClaims
            );

            string token = new JwtSecurityTokenHandler().WriteToken(newJwtToken);
            string refreshToken = GenerateRefreshToken();

            return new TokenModel(token, refreshToken);
        }
        private void UpdateRefreshTokenInDB(string username, string token, string refreshtoken, string newrefreshtoken = "")
        {
            dm.ExecScalarSP("sp_UserUpdateRefreshToken",
                "Username", username ?? "",
                "Token", token ?? "",
                "RefreshToken", refreshtoken ?? "",
                "NewRefreshToken", newrefreshtoken ?? "");
        }
        private string GenerateRefreshToken()
        {
            var key = new Byte[32];
            using (var refreshTokenGenerator = RandomNumberGenerator.Create())
            {
                refreshTokenGenerator.GetBytes(key);
                return Convert.ToBase64String(key);
            }
        }
        private List<string> GetUserPermissions(string? username)
        {
            List<string> permissions = new List<string>();

            var userPermissionsDt = dm.ExecDataTableSP("sp_UserPermissions", "Username", username ?? "");

            foreach (DataRow permission in userPermissionsDt.Rows)
            {
                permissions.Add(permission[0].ToString() ?? "");
            }

            return permissions;
        }
        public UsersGetDTO UsersGet(PagingSortingFilteringRequest request)
        {
            var usersCountResult = dm.ExecScalarSP("sp_UsersCountGet");
            var response = new UsersGetDTO();

            response.TotalRows = (int)usersCountResult.Result;
            if (request != null)
            {
                response.PageSize = request.PageSize;
                response.CurrentPage = request.CurrentPage;
            }

            var list = new List<UserDTO>();
            var UserListDt = dm.ExecDataTableSP("sp_UserList", "PageSize", response.PageSize, "OffSet", response.OffSet());
            foreach (DataRow users in UserListDt.Rows)
            {
                list.Add(new UserDTO()
                {
                    Id = int.Parse(users["Id"].ToString() ?? ""),
                    Name = users["Name"].ToString(),
                    Username = users["Username"].ToString(),
                    Email = users["Email"].ToString()
                });
            }
            response.UserList = list;
            return response;
        }
        public APIResult UserCreate(UserCreateModel user)
        {
            APIResult result = new APIResult(user);

            result.ModelValidationResult = user.ValidateModel();

            if (!result.ModelValidationResult.IsValid)
            {
                result.HasError = true;
                result.Message = "See Model Validations";
                return result;
            }

            var UserExists = UserVerifyExistsByUsername(user.Username);
            if (UserExists)
            {
                result.Message = "Username already exists.";
                result.HasError = true;
                return result;
            }

            var EmailExists = UserVerifyExistsByEmail(user.Email);
            if (EmailExists)
            {
                result.Message = "Email already exists.";
                result.HasError = true;
                return result;
            }

            var dbresult = dm.ExecScalarSP("sp_UserCreate", "Username", user.Username ?? "", "Email", user.Email ?? "", "Name", user.FullName ?? "");
            result.Result = dbresult.Result;
            result.HasError = dbresult.HasError;
            result.Exception = dbresult.Exception;
            result.Message = "User Created";

            return result;
        }
        public bool UserVerifyExistsByUsername(string username)
        {
            var dt = dm.ExecDataTableSP("sp_UserList", "Username", username);

            if (dt.Rows.Count == 0)
                return false;

            return true;
        }
        public bool UserVerifyExistsByEmail(string email)
        {
            var dt = dm.ExecDataTableSP("sp_UserList", "Email", email);

            if (dt.Rows.Count == 0)
                return false;

            return true;
        }
        public APIResult UserCreateLocalPassword(UserCreateLocalPasswordModel u)
        {
            //TODO : Implement Password Complexity Rules
            //TODO : Implement Previously Used Password Constraint

            APIResult result = new APIResult(u);

            var ramdomSalt = SecurityHelpers.RandomizeSalt;

            var dbresult = dm.ExecScalarSP("sp_UserPasswordCreate",
                "UserId", u.UserId,
                "HashedPassword", SecurityHelpers.HashPasword(u.Password, ramdomSalt),
                "Salt", Convert.ToHexString(ramdomSalt));
            result.Result = dbresult.Result;
            result.HasError = dbresult.HasError;
            result.Exception = dbresult.Exception;
            result.Message = "Password Created";

            return result;
        }
        public List<PermissionModel> PermissionsGet()
        {
            var list = new List<PermissionModel>();
            var permissionsGetDt = dm.ExecDataTableSP("sp_PermissionList");
            foreach (DataRow permission in permissionsGetDt.Rows)
            {
                list.Add(new PermissionModel()
                {
                    Id = int.Parse(permission["Id"].ToString() ?? ""),
                    PermissionName = permission["PermissionName"].ToString(),
                    PermissionDescription = permission["PermissionDescription"].ToString()
                });
            }

            return list;
        }
        public APIResult PermissionCreate(PermissionCreateModel model)
        {
            APIResult result = new APIResult(model);

            result.ModelValidationResult = model.ValidateModel();

            if (!result.ModelValidationResult.IsValid)
            {
                result.HasError = true;
                result.Message = "See Model Validations";
                return result;
            }

            var UserExists = PermissionVerifyExistsByPermissionName(model.PermissionName);
            if (UserExists)
            {
                result.Message = "Permission already exists.";
                result.HasError = true;
                return result;
            }

            var dbresult = dm.ExecScalarSP("sp_PermissionCreate", "PermissionName", model.PermissionName, "PermissionDescription", model.PermissionDescription);
            result.Result = dbresult.Result;
            result.HasError = dbresult.HasError;
            result.Exception = dbresult.Exception;
            result.Message = "Permission Created";

            return result;
        }
        public bool PermissionVerifyExistsByPermissionName(string permissionName)
        {
            var dt = dm.ExecDataTableSP("sp_PermissionList", "PermissionName", permissionName);

            if (dt.Rows.Count == 0)
                return false;

            return true;
        }
        public List<RoleModel> RolesGet()
        {
            var list = new List<RoleModel>();
            var rolesGetDt = dm.ExecDataTableSP("sp_RoleList");
            foreach (DataRow role in rolesGetDt.Rows)
            {
                list.Add(new RoleModel()
                {
                    Id = int.Parse(role["Id"].ToString() ?? ""),
                    RoleName = role["RoleName"].ToString(),
                    RoleDescription = role["RoleDescription"].ToString()
                });
            }
            return list;
        }
        public APIResult RoleCreate(RoleCreateModel model)
        {
            APIResult result = new APIResult(model);

            result.ModelValidationResult = model.ValidateModel();

            if (!result.ModelValidationResult.IsValid)
            {
                result.HasError = true;
                result.Message = "See Model Validations";
                return result;
            }

            var UserExists = RoleVerifyExistsByRoleName(model.RoleName);
            if (UserExists)
            {
                result.Message = "Role already exists.";
                result.HasError = true;
                return result;
            }

            var dbresult = dm.ExecScalarSP("sp_RoleCreate", "RoleName", model.RoleName, "RoleDescription", model.RoleDescription);
            result.Result = dbresult.Result;
            result.HasError = dbresult.HasError;
            result.Exception = dbresult.Exception;
            result.Message = "Role Created";

            return result;
        }

        public APIResult RoleUpdate(RoleModel model)
        {
            APIResult result = new APIResult(model);

            result.ModelValidationResult = model.ValidateModel();

            if (!result.ModelValidationResult.IsValid)
            {
                result.HasError = true;
                result.Message = "See Model Validations";
                return result;
            }

            var UserExists = RoleVerifyExistsByRoleName(model.RoleName);
            if (UserExists)
            {
                result.Message = "Role already exists.";
                result.HasError = true;
                return result;
            }

            var dbresult = dm.ExecScalarSP("sp_RoleUpdate", "Id", model.Id, "RoleName", model.RoleName, "RoleDescription", model.RoleDescription);
            result.Result = dbresult.Result;
            result.HasError = dbresult.HasError;
            result.Exception = dbresult.Exception;
            result.Message = "Role Updated";

            return result;
        }

        public bool RoleVerifyExistsByRoleName(string roleName)
        {
            var dt = dm.ExecDataTableSP("sp_RoleList", "RoleName", roleName);

            if (dt.Rows.Count == 0)
                return false;

            return true;
        }
        public List<RolePermissionModel> RolePermissionsGet()
        {
            var list = new List<RolePermissionModel>();
            var dt = dm.ExecDataTableSP("sp_RolePermissionsGet");
            foreach (DataRow role in dt.Rows)
            {
                list.Add(new RolePermissionModel()
                {
                    RoleId = int.Parse(role["RoleId"].ToString() ?? ""),
                    RoleName = role["RoleName"].ToString(),
                    RoleDescription = role["RoleDescription"].ToString(),
                    PermissionId = int.Parse(role["PermissionId"].ToString() ?? ""),
                    PermissionName = role["PermissionName"].ToString(),
                    PermissionDescription = role["PermissionDescription"].ToString()
                });
            }

            return list;
        }
        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _tokenSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _tokenSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key)),
                    ValidateLifetime = false
                }, out SecurityToken validatedToken);


                var jwtToken = validatedToken as JwtSecurityToken;

                if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                    return null;

                return claimsPrincipal;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public string GetUsernameFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
                return null;

            var username = claimsPrincipal.Claims.Where(_ => _.Type == "UserName").Select(_ => _.Value).FirstOrDefault();
            //var email = claimsPrincipal.Claims.Where(_ => _.Type == ClaimTypes.Email).Select(_ => _.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(username))
                return null;

            return username;
        }
        public TokenModel RefreshToken(TokenModel tokenModel)
        {
            //implement method that declines renewal depeding of token age.

            var claimsPrincipal = ValidateToken(tokenModel.Token);
            if (claimsPrincipal == null)
                return null;

            var username = GetUsernameFromClaimsPrincipal(claimsPrincipal);
            if (username == null)
                return null;

            string refreshToken = dm.ExecScalarSP("sp_UserGetRefreshToken", "Username", username, "RefreshToken", tokenModel.RefreshToken).Result.ToString();
            if (refreshToken == null || refreshToken != tokenModel.RefreshToken)
                return null;

            var newtoken = GenerateAuthenticationToken(username);
            UpdateRefreshTokenInDB(username, newtoken.Token, tokenModel.RefreshToken, newtoken.RefreshToken);
            return newtoken;
        }
    }
}
