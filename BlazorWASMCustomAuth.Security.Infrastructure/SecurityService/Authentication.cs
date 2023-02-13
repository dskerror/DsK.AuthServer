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

        public async Task<APIResult<TokenModel>> UserLogin(UserLoginDto model)
        {
            APIResult<TokenModel> result = new APIResult<TokenModel>();
            bool IsUserAuthenticated = AuthenticateUser(model);

            if (!IsUserAuthenticated)
                return null;

            var token = await GenerateAuthenticationToken(model.Username);
            
            db.UserTokens.Add(new UserToken()
            {
                Token = token.Token,
                RefreshToken = token.RefreshToken,
                TokenRefreshedDateTime = DateTime.Now,
                TokenCreatedDateTime= DateTime.Now
            });

            await db.SaveChangesAsync();

            result.Result = token;
            return result;
        }
        public async Task<APIResult<UserTokenDto>> RefreshToken(TokenModel model)
        {
            APIResult<UserTokenDto> result = new APIResult<UserTokenDto>();
            //implement method that declines renewal depeding of token age.

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


            var userToken = await db.UserTokens.Where(x => x.RefreshToken == model.RefreshToken).FirstOrDefaultAsync();

            if (userToken == null || userToken.RefreshToken != model.RefreshToken)
            {
                result.HasError = true;
                result.Message = "Invalid Token";
                return result;
            }

            var newtoken = await GenerateAuthenticationToken(username);
            userToken.RefreshToken = newtoken.RefreshToken;
            userToken.Token = newtoken.Token;
            userToken.TokenRefreshedDateTime = DateTime.Now;
            await db.SaveChangesAsync();

            UserTokenDto userTokenDto = new UserTokenDto();
            Mapper.Map(userToken, userTokenDto);

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
        private async Task<TokenModel> GenerateAuthenticationToken(string username)
        {
            if (username.IsNullOrEmpty())
                return null;
            var user = await db.Users.Where(x=>x.Username == username).FirstOrDefaultAsync();
            
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key ?? ""));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var userPermissions = await GetUserPermissionsCombined(user.Id);
            var userClaims = new List<Claim>();
            userClaims.Add(new Claim(ClaimTypes.Email, user.Email ?? ""));
            userClaims.Add(new Claim("UserId", user.Id.ToString()));
            userClaims.Add(new Claim("UserName", user.Username ?? ""));

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
        private bool AuthenticateUser(UserLoginDto model)
        {
            bool IsUserAuthenticated = false;
            switch (model.AuthenticationProviderName)
            {
                case "Active Directory":
                    IsUserAuthenticated = AuthenticateUserWithDomain(model.Username, model.Password);
                    break;
                default: //Local
                    IsUserAuthenticated = AuthenticateUserWithLocalPassword(model.Username, model.Password);
                    break;
            }
            return IsUserAuthenticated;
        }
        private bool AuthenticateUserWithLocalPassword(string username, string password)
        {
            try
            {
                var userPassword = db.UserPasswords.Where(x => x.User.Username == username).FirstOrDefault();

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
        private bool AuthenticateUserWithDomain(string username, string password)
        {
            //TODO: Get AD credentials

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
    }
}
