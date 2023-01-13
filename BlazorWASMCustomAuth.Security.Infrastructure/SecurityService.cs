﻿using BlazorWASMCustomAuth.Database;
using BlazorWASMCustomAuth.PagingSortingFiltering;
using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Shared.Security;
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
            var user = GetUser(model.Username, model.Password);

            if (user is null)
                return new TokenModel("", "");
            else if (AuthenticateUser(user))
            {
                var token = GenerateAuthenticationToken(user);
                UpdateRefreshTokenInDB(user.Username, token.Token, user.RefreshToken);
                return token;
            }
            else
            {
                return new TokenModel("", "");
            }
        }
        private UserModel GetUser(string username, string password = "", int userid = 0)
        {
            var userDt = dm.ExecDataTableSP("sp_UserGet", "Username", username ?? "", "Id", userid);

            if (userDt.Rows.Count == 1)
            {
                UserModel user = new UserModel()
                {
                    Id = int.Parse(userDt.Rows[0]["id"].ToString() ?? ""),
                    Username = userDt.Rows[0]["Username"].ToString(),
                    Password = password,
                    HashedPassword = userDt.Rows[0]["HashedPassword"].ToString(),
                    Salt = userDt.Rows[0]["Salt"].ToString(),
                    AuthenticationProviderName = userDt.Rows[0]["AuthenticationProviderName"].ToString(),
                    MappedUser = userDt.Rows[0]["MappedUsername"].ToString(),
                    RefreshToken = userDt.Rows[0]["RefreshToken"].ToString(),
                    Permissions = GetUserPermissions(userDt.Rows[0]["Username"].ToString())
                };
                return user;
            }
            return new UserModel();
        }

        private bool AuthenticateUser(UserModel user)
        {
            if (user is null) return false;

            if (user.AuthenticationProviderName == "Active Directory")
                return ValidateWithDomain(user.MappedUser ?? "", user.Password ?? "");
            else
                return VerifyPassword(user.Password ?? "", user.HashedPassword ?? "", user.Salt ?? "");
        }

        bool VerifyPassword(string password, string hash, string salt)
        {
            byte[] bytesalt = Convert.FromHexString(salt);
            const int keySize = 64;
            const int iterations = 350000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, bytesalt, iterations, hashAlgorithm, keySize);
            return hashToCompare.SequenceEqual(Convert.FromHexString(hash));
        }

        private bool ValidateWithDomain(string username, string password)
        {
#pragma warning disable IDE0063 // Use simple 'using' statement
#pragma warning disable CA1416 // Validate platform compatibility
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "DOMAIN", "USERNAME", "PASSWORD"))
            {
                // validate the credentials
                bool isValid = pc.ValidateCredentials(username, password);
                return isValid;
            }
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore IDE0063 // Use simple 'using' statement
        }

        private TokenModel GenerateAuthenticationToken(UserModel user)
        {
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

            user.RefreshToken = refreshToken;

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
            var UserListDt = dm.ExecDataTableSP("sp_UsersGet", "PageSize", response.PageSize, "OffSet", response.OffSet());
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


        public DatabaseExecResult UserCreate(UserCreateModel user)
        {
            var userExistsDT = dm.ExecDataTableSP("sp_UserCreate_VerifyIfExists", "Username", user.Username ?? "", "Email", user.Email ?? "");

            if (userExistsDT.Rows.Count == 0)
            {
                return dm.ExecScalarSP("sp_UserCreate", "Username", user.Username ?? "", "Email", user.Email ?? "", "Name", user.Name ?? "");
            }

            return new DatabaseExecResult("") { HasError = true, Message = "User already exists.", Exception = null};
        }

        public DatabaseExecResult UserCreateLocalPassword(UserCreateLocalPasswordModel u)
        {
            //TODO : Implement Password Complexity Rules
            //TODO : Implement Previously Used Password Constraint

            var ramdomSalt = SecurityHelpers.RandomizeSalt;

            return dm.ExecScalarSP("sp_UserPasswordCreate", 
                "UserId", u.UserId,
                "HashedPassword", SecurityHelpers.HashPasword(u.Password, ramdomSalt), 
                "Salt", Convert.ToHexString(ramdomSalt));
        }


        public DatabaseExecResult UserChangeLocalPassword(UserChangeLocalPasswordModel u)
        {
            //TODO : Implement Password Complexity Rules
            //TODO : Implement Previously Used Password Constraint
    
            var user = GetUser("",u.OldPassword,u.UserId);

            if (user is null)
                return new DatabaseExecResult("") { HasError = true, Message = "User or password incorrect." };
            else if (AuthenticateUser(user))
            {
                var ramdomSalt = SecurityHelpers.RandomizeSalt;

                return dm.ExecScalarSP("sp_UserPasswordChange",
                    "UserId", u.UserId,
                    "HashedPassword", SecurityHelpers.HashPasword(u.NewPassword, ramdomSalt),
                    "Salt", Convert.ToHexString(ramdomSalt));
            } else
            {
                return new DatabaseExecResult("") { HasError = true, Message = "User or password incorrect" };
            }
        
        }
        //public List<PermissionModel> GetPermissionList()
        //{
        //	var list = new List<PermissionModel>();
        //	var permissionListDt = dm.ExecDataTableSP("sp_PermissionList");
        //	foreach (DataRow permission in permissionListDt.Rows)
        //	{
        //		list.Add(new PermissionModel()
        //		{
        //			Id = int.Parse(permission["Id"].ToString() ?? ""),
        //			PermissionName = permission["PermissionName"].ToString(),
        //			PermissionDescription = permission["PermissionDescription"].ToString()
        //		});
        //	}

        //	return list;
        //}
        //public List<RoleModel> GetRoleList()
        //{
        //	var list = new List<RoleModel>();
        //	var roleListDt = dm.ExecDataTableSP("sp_RoleList");
        //	foreach (DataRow role in roleListDt.Rows)
        //	{
        //		list.Add(new RoleModel()
        //		{
        //			Id = int.Parse(role["Id"].ToString() ?? ""),
        //			RoleName = role["RoleName"].ToString(),
        //			RoleDescription = role["RoleDescription"].ToString()
        //		});
        //	}

        //	return list;
        //}
        //public List<RolePermissionModel> RolePermissionList()
        //{
        //	var list = new List<RolePermissionModel>();
        //	var rolePermissionListDt = dm.ExecDataTableSP("sp_RolePermissionList");
        //	foreach (DataRow role in rolePermissionListDt.Rows)
        //	{
        //		list.Add(new RolePermissionModel()
        //		{
        //			RoleId = int.Parse(role["RoleId"].ToString() ?? ""),
        //			RoleName = role["RoleName"].ToString(),
        //			RoleDescription = role["RoleDescription"].ToString(),
        //			PermissionId = int.Parse(role["PermissionId"].ToString() ?? ""),
        //			PermissionName = role["PermissionName"].ToString(),
        //			PermissionDescription = role["PermissionDescription"].ToString()
        //		});
        //	}

        //	return list;
        //}

        public TokenModel RefreshToken(TokenModel tokenModel)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsPrincipal = tokenHandler.ValidateToken(tokenModel.Token,
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
            {
                return null;
            }

            var username = claimsPrincipal.Claims.Where(_ => _.Type == "UserName").Select(_ => _.Value).FirstOrDefault();
            //var email = claimsPrincipal.Claims.Where(_ => _.Type == ClaimTypes.Email).Select(_ => _.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }


            var user = GetUser(username);

            if (user.RefreshToken == tokenModel.RefreshToken)
            {   
                var newtoken =  GenerateAuthenticationToken(user);
                UpdateRefreshTokenInDB(user.Username, newtoken.Token, tokenModel.RefreshToken, newtoken.RefreshToken);
                return newtoken;
            }
            else
            {
                return null;
            }
            //var currentUser = _myWorldDbContext.User.Where(_ => _.Email == email && _.RefreshToken == tokenModel.RefreshToken).FirstOrDefault();
            //if (currentUser == null)
            //{
            //    return null;
            //}


        }

    }
}
