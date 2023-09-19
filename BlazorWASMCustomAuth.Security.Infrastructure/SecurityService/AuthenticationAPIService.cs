using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {

        public async Task<APIResult<string>> ApplicationLoginRequest(ApplicationLoginRequestDto model)
        {
            APIResult<string> result = new APIResult<string>();
            

            db.UserTokens.Add(new UserToken()
            {
                UserId = user.Id,
                RefreshToken = token.RefreshToken,
                TokenRefreshedDateTime = DateTime.Now,
                TokenCreatedDateTime = DateTime.Now
            });

            await db.SaveChangesAsync();

            result.Result = token;
            return result;
        }

        public async Task<APIResult<TokenModel>> UserLogin(UserLoginDto model)
        {
            APIResult<TokenModel> result = new APIResult<TokenModel>();
            var user = await AuthenticateUser(model);

            if (user == null)
            {
                result.HasError = true;
                return result;
            }

            var token = await GenerateAuthenticationToken(user);

            db.UserTokens.Add(new UserToken()
            {
                UserId = user.Id,
                RefreshToken = token.RefreshToken,
                TokenRefreshedDateTime = DateTime.Now,
                TokenCreatedDateTime = DateTime.Now
            });

            await db.SaveChangesAsync();

            result.Result = token;
            return result;
        }
        public async Task<APIResult<UserTokenDto>> RefreshToken(TokenModel model)
        {
            APIResult<UserTokenDto> result = new APIResult<UserTokenDto>();

            var claimsPrincipal = ValidateToken(model.Token);
            if (claimsPrincipal == null)
            {
                result.HasError = true;
                result.Message = "Invalid Token";
                return result;
            }


            var username = GetUsernameFromClaimsPrincipal(claimsPrincipal);
            if (username == null)
            {
                result.HasError = true;
                result.Message = "Invalid Token";
                return result;
            }

            //TODO : Remove token from UserToken Table            
            //TODO : Create cleanup method that remove refreshtokens older than established date

            var user = await GetUserByEmailAsync(username);
            if (user == null)
            {
                result.HasError = true;
                result.Message = "Invalid Token";
                return result;
            }

            var userToken = await db.UserTokens.Where(x => x.RefreshToken == model.RefreshToken && x.UserId == user.Id).FirstOrDefaultAsync();

            if (userToken == null || userToken.RefreshToken != model.RefreshToken)
            {
                result.HasError = true;
                result.Message = "Invalid Token";
                return result;
            }

            //Don't refresh if token is x time.
            if (userToken.TokenRefreshedDateTime < DateTime.Now.AddDays(-1))
            {
                result.HasError = true;
                result.Message = "Expired Refresh Token";
                return result;
            }

            var newtoken = await GenerateAuthenticationToken(user);
            userToken.UserId = user.Id;
            userToken.RefreshToken = newtoken.RefreshToken;
            userToken.TokenRefreshedDateTime = DateTime.Now;
            await db.SaveChangesAsync();

            UserTokenDto userTokenDto = new UserTokenDto()
            {
                Token = newtoken.Token,
                RefreshToken = newtoken.RefreshToken,
            };


            result.Result = userTokenDto;
            result.Message = "Token Refreshed";
            return result;
        }

        private ClaimsPrincipal ValidateToken(string token)
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
        private string GetUsernameFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
                return null;

            var username = claimsPrincipal.Claims.Where(_ => _.Type == "UserName").Select(_ => _.Value).FirstOrDefault();
            //var email = claimsPrincipal.Claims.Where(_ => _.Type == ClaimTypes.Email).Select(_ => _.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(username))
                return null;

            return username;
        }
        private async Task<TokenModel> GenerateAuthenticationToken(User user)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key ?? ""));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var userPermissions = await GetUserPermissionsCombined(user.Id);
            var userClaims = new List<Claim>();
            userClaims.Add(new Claim(ClaimTypes.Email, user.Email ?? ""));
            userClaims.Add(new Claim("UserId", user.Id.ToString()));            

            foreach (var permission in userPermissions)
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
        private string GenerateRefreshToken()
        {
            var key = new Byte[32];
            using (var refreshTokenGenerator = RandomNumberGenerator.Create())
            {
                refreshTokenGenerator.GetBytes(key);
                return Convert.ToBase64String(key);
            }
        }
        private async Task<User> AuthenticateUser(UserLoginDto model)
        {
            var user = await GetUserByMappedUsernameAsync(model.Email, model.AuthenticationProviderId);

            bool IsUserAuthenticated = false;

            var applicationAuthenticationProvider = await ApplicationApplicationAuthenticationProviderGet(model.AuthenticationProviderId);

            switch (applicationAuthenticationProvider.AuthenticationProviderType)
            {
                case "Active Directory":
                    IsUserAuthenticated = AuthenticateUserWithDomain(model.Email, model.Password, applicationAuthenticationProvider);
                    if (IsUserAuthenticated)
                        user = await CreateADUserIfNotExists(model, user, applicationAuthenticationProvider);
                    break;
                default: //Local
                    IsUserAuthenticated = await AuthenticateUserWithLocalPassword(model.Email, model.Password);
                    break;
            }
            if (IsUserAuthenticated)
                return user;
            else
                return null;
        }

        private async Task<User> CreateADUserIfNotExists(UserLoginDto model, User? user, ApplicationAuthenticationProvider applicationAuthenticationProvider)
        {
            if (user == null)
            {
                UserCreateDto userCreateDto = new UserCreateDto()
                {
                    Email = GetADUserEmail(model.Email, applicationAuthenticationProvider),
                    Name = GetADUserDisplayName(model.Email, applicationAuthenticationProvider)
                };

                var result = await UserCreate(userCreateDto);

                UserAuthenticationProviderCreateDto userAuthenticationProviderCreateDto = new UserAuthenticationProviderCreateDto()
                {
                    Username = model.Email,
                    AuthenticationProviderId = model.AuthenticationProviderId,
                    UserId = result.Result.Id
                };

                UserAuthenticationProviderCreate(userAuthenticationProviderCreateDto);
                user = await GetUserByMappedUsernameAsync(model.Email, model.AuthenticationProviderId);
            }
            return user;
        }

        private async Task<bool> AuthenticateUserWithLocalPassword(string username, string password)
        {
            try
            {
                var userPassword = await db.UserPasswords.Where(x => x.User.Email == username).OrderByDescending(x => x.Id).FirstOrDefaultAsync();

                if (userPassword == null)
                    return false;

                byte[] bytesalt = Convert.FromHexString(userPassword.Salt);
                const int keySize = 64;
                const int iterations = 350000;
                HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
                var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, bytesalt, iterations, hashAlgorithm, keySize);
                return hashToCompare.SequenceEqual(Convert.FromHexString(userPassword.HashedPassword));

            }
            catch (Exception)
            {
                return false;
            }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        private bool AuthenticateUserWithDomain(string username, string password, ApplicationAuthenticationProvider ApplicationAuthenticationProvider)
        {
            //todo : encrypt Authentication Provider password 
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, ApplicationAuthenticationProvider.Domain, ApplicationAuthenticationProvider.Username, ApplicationAuthenticationProvider.Password))
            {
                // validate the credentials
                bool isValid = pc.ValidateCredentials(username, password);
                return isValid;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        private string GetADUserEmail(string username, ApplicationAuthenticationProvider ApplicationAuthenticationProvider)
        {   
            string email = "";
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, ApplicationAuthenticationProvider.Domain, ApplicationAuthenticationProvider.Username, ApplicationAuthenticationProvider.Password))
            {
                UserPrincipal user = UserPrincipal.FindByIdentity(pc, username);
                email = user.EmailAddress;
            }
            return email;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public string GetADUserDisplayName(string username, ApplicationAuthenticationProvider ApplicationAuthenticationProvider)
        {   
            string realname = "";
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, ApplicationAuthenticationProvider.Domain, ApplicationAuthenticationProvider.Username, ApplicationAuthenticationProvider.Password))
            {
                UserPrincipal user = UserPrincipal.FindByIdentity(pc, username);
                if (user != null)
                    realname = user.DisplayName;
            }
            return realname;
        }
    }
}
