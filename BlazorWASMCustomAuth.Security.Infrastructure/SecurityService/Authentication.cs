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
    public partial class SecurityService
    {
        
        public TokenModel UserLogin(UserLoginModel model)
        {
            if (model.Username.IsNullOrEmpty() || model.Password.IsNullOrEmpty())
                return null;

            bool IsUserAuthenticated = AuthenticateUser(model);

            if (!IsUserAuthenticated)
                return null;

            var token = GenerateAuthenticationToken(model.Username);
            UpdateRefreshTokenInDB(model.Username, token.Token, token.RefreshToken);
            return token;
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

            string refreshToken = dm.ExecScalarSP("sp_UserTokenList", "RefreshToken", tokenModel.RefreshToken).Result.ToString();
            if (refreshToken == null || refreshToken != tokenModel.RefreshToken)
                return null;

            var newtoken = GenerateAuthenticationToken(username);
            UpdateRefreshTokenInDB(username, newtoken.Token, tokenModel.RefreshToken, newtoken.RefreshToken);
            return newtoken;
        }
   


        private TokenModel GenerateAuthenticationToken(string username)
        {
            if (username.IsNullOrEmpty())
                return null;

            var user = UserGetBy("username", username);

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
        private string GenerateRefreshToken()
        {
            var key = new Byte[32];
            using (var refreshTokenGenerator = RandomNumberGenerator.Create())
            {
                refreshTokenGenerator.GetBytes(key);
                return Convert.ToBase64String(key);
            }
        }
        private bool AuthenticateUser(UserLoginModel model)
        {
            if (model.Username.IsNullOrEmpty() || model.Password.IsNullOrEmpty())
                return false;

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
        private void UpdateRefreshTokenInDB(string username, string token, string refreshtoken, string newrefreshtoken = "")
        {
            dm.ExecScalarSP("sp_UserTokenUpdate",
                "Username", username ?? "",
                "Token", token ?? "",
                "RefreshToken", refreshtoken ?? "",
                "NewRefreshToken", newrefreshtoken ?? "");
        }
    }
}
