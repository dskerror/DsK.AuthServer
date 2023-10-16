using DsK.AuthServer.Security.EntityFramework.Models;
using DsK.AuthServer.Security.Shared;
using DsK.AuthServer.Security.Shared.ActionDtos;
using DsK.Services;
using DsK.Services.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace DsK.AuthServer.Security.Infrastructure;

public partial class SecurityService
{
    public async Task<TokenModel> ValidateLoginToken(ValidateLoginTokenDto model)
    {
        var userToken = db.ApplicationAuthenticationProviderUserTokens.Where(x => x.LoginToken == Guid.Parse(model.LoginToken)).Include(x => x.User).FirstOrDefault();

        if (model.TokenKey == "")
            model.TokenKey = _tokenSettings.Key;

        var token = await GenerateAuthenticationToken(userToken.User, model.TokenKey);

        userToken.RefreshToken = token.RefreshToken;
        await db.SaveChangesAsync();

        return token;
    }
    public async Task<APIResult<LoginResponseDto>> Login(LoginRequestDto model)
    {
        APIResult<LoginResponseDto> apiResult = new APIResult<LoginResponseDto>();
        var applicationAuthenticationProvider = await ApplicationAuthenticationProviderGet(model.ApplicationAuthenticationProviderGUID);

        if (!applicationAuthenticationProvider.IsEnabled)
        {
            apiResult.HasError = true;
            apiResult.Message = "This Application Authentication Provider Is Disabled";
            return apiResult;
        }

        var user = await AuthenticateUser(model, applicationAuthenticationProvider);
        if (user == null)
        {
            apiResult.HasError = true;
            apiResult.Message = "Username and/or Password incorrect";
            return apiResult;
        }

        if (!user.EmailConfirmed)
        {
            apiResult.HasError = true;
            apiResult.Message = "Email hasn't been confirmed yet.";
            return apiResult;
        }

        string CallbackURL = applicationAuthenticationProvider.Application.CallbackUrl;

        var newguid = Guid.NewGuid();

        db.ApplicationAuthenticationProviderUserTokens.Add(new ApplicationAuthenticationProviderUserToken()
        {
            ApplicationId = applicationAuthenticationProvider.Application.Id,
            UserId = user.Id,
            LoginToken = newguid,
            TokenCreatedDateTime = DateTime.Now,
            TokenRefreshedDateTime = DateTime.Now,
            RefreshToken = ""
        });

        CallbackURL += newguid;

        try
        {
            apiResult.HasError = false;
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            apiResult.HasError = true;
            apiResult.Message = ex.Message;
        }

        var loginResponseDto = new LoginResponseDto();
        loginResponseDto.CallbackURL = CallbackURL;
        loginResponseDto.LoginToken = newguid;

        apiResult.Result = loginResponseDto;

        return apiResult;
    }
    public async Task<APIResult<string>> Register(RegisterRequestDto model, string origin)
    {
        APIResult<string> apiResult = new APIResult<string>() { HasError = true };
        bool userAlreadyExists = false;
        var applicationAuthenticationProvider = await ApplicationAuthenticationProviderGet(model.ApplicationAuthenticationProviderGUID);
        var user = db.Users.Where(x => x.Email == model.Email).FirstOrDefault();

        if (!applicationAuthenticationProvider.RegistrationEnabled && !applicationAuthenticationProvider.ActiveDirectoryFirstLoginAutoRegister)
        {
            apiResult.HasError = true;
            apiResult.Message = "Registration is disabled for this Application Authentication Provider";
            return apiResult;
        }

        if (user == null)
            user = await CreateUser(model, applicationAuthenticationProvider, user);
        else
            userAlreadyExists = true;

        await RegisterAuthApp(model, origin, applicationAuthenticationProvider);
        await CreateApplicationUser(applicationAuthenticationProvider, user);
        await AddDefaultRoleToUser(applicationAuthenticationProvider, user);
        await AddApplicationAuthenticationProviderUserMapping(model, applicationAuthenticationProvider, user);
        await SendRegistrationEmail(origin, applicationAuthenticationProvider, user).ConfigureAwait(false);

        if (user.Id != 0)
        {
            apiResult.HasError = false;
            if (!user.EmailConfirmed)
            {
                apiResult.Message = "User has been created. An email has been sent to the user for confirmation.";
                return apiResult;
            }

            if (userAlreadyExists)
            {
                apiResult.Message = "User already exists and has been added to application.";
                return apiResult;
            }

            if (!userAlreadyExists && user.EmailConfirmed)
            {
                apiResult.Message = "User has been created. Please login to access application.";
                return apiResult;
            }
        }

        return apiResult;
    }

    private async Task RegisterAuthApp(RegisterRequestDto model, string origin, ApplicationAuthenticationProvider applicationAuthenticationProvider)
    {
        if (applicationAuthenticationProvider.Id != 1)
        {
            RegisterRequestDto newModel = new RegisterRequestDto()
            {
                Name = model.Name,
                ADUsername = model.ADUsername,
                ApplicationAuthenticationProviderGUID = Guid.Parse("67A09D32-7664-49F8-8E8E-471A56A2858E"),
                Email = model.Email,
                Password = model.Password
            };
            await Register(newModel, origin);
        }
    }
    private async Task SendRegistrationEmail(string origin, ApplicationAuthenticationProvider applicationAuthenticationProvider, User? user)
    {
        if (user.Id != 0 && !user.EmailConfirmed)
        {
            //send email
            if (!applicationAuthenticationProvider.RegistrationAutoEmailConfirm)
            {
                var verificationUri = $"{origin}/EmailConfirm/{user.Salt}";
                var mailRequest = new MailRequest
                {
                    From = "noreply@dsk.com",
                    To = user.Email,
                    Body = $"Please confirm your account by <a href='{verificationUri}'>clicking here</a>.",
                    Subject = "Confirm Registration"
                };
                await _mailService.SendAsync(mailRequest).ConfigureAwait(false);
            }
        }
    }
    private async Task AddApplicationAuthenticationProviderUserMapping(RegisterRequestDto model, ApplicationAuthenticationProvider applicationAuthenticationProvider, User? user)
    {
        if (user.Id != 0)
        {
            var applicationAuthenticationProviderUserMapping = await db.ApplicationAuthenticationProviderUserMappings.Where(x => x.UserId == user.Id && x.ApplicationAuthenticationProviderId == applicationAuthenticationProvider.Id).FirstOrDefaultAsync();
            if (applicationAuthenticationProviderUserMapping == null)
            {
                var username = user.Email;
                if (applicationAuthenticationProvider.AuthenticationProviderType == "Active Directory")
                    username = model.ADUsername;

                applicationAuthenticationProviderUserMapping = new ApplicationAuthenticationProviderUserMapping()
                {
                    UserId = user.Id,
                    ApplicationAuthenticationProviderId = applicationAuthenticationProvider.Id,
                    Username = username
                };

                db.ApplicationAuthenticationProviderUserMappings.Add(applicationAuthenticationProviderUserMapping);

                await db.SaveChangesAsync();
            }
        }
    }
    private async Task AddDefaultRoleToUser(ApplicationAuthenticationProvider applicationAuthenticationProvider, User? user)
    {
        if (user.Id != 0)
        {
            if (applicationAuthenticationProvider.DefaultApplicationRoleId != null)
            {
                var userRoles = await db.UserRoles.Where(x => x.UserId == user.Id && x.RoleId == applicationAuthenticationProvider.DefaultApplicationRoleId).FirstOrDefaultAsync();
                if (userRoles == null)
                {
                    var userRole = new UserRole()
                    {
                        RoleId = (int)applicationAuthenticationProvider.DefaultApplicationRoleId,
                        UserId = user.Id,
                    };

                    db.UserRoles.Add(userRole);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
    private async Task CreateApplicationUser(ApplicationAuthenticationProvider applicationAuthenticationProvider, User? user)
    {
        if (user.Id != 0)
        {
            var applicationUser = await db.ApplicationUsers.Where(x => x.UserId == user.Id && x.ApplicationId == applicationAuthenticationProvider.Id).FirstOrDefaultAsync();
            if (applicationUser == null)
            {
                applicationUser = new ApplicationUser()
                {
                    UserId = user.Id,
                    ApplicationId = applicationAuthenticationProvider.ApplicationId
                };

                db.ApplicationUsers.Add(applicationUser);
                await db.SaveChangesAsync();
            }
        }
    }
    private async Task<User?> CreateUser(RegisterRequestDto model, ApplicationAuthenticationProvider applicationAuthenticationProvider, User? user)
    {
        if (user == null)
        {
            var ramdomSalt = SecurityHelpers.RandomizeSalt;
            if (model.Password == null)
                model.Password = Convert.ToHexString(ramdomSalt);

            model.Email ??= $"{Convert.ToHexString(ramdomSalt)}@noemail.com";

            user = new User()
            {
                Email = model.Email,
                Name = model.Name,
                EmailConfirmed = applicationAuthenticationProvider.RegistrationAutoEmailConfirm,
                AccessFailedCount = 0,
                LockoutEnabled = false,
                HashedPassword = SecurityHelpers.HashPasword(model.Password, ramdomSalt),
                Salt = Convert.ToHexString(ramdomSalt),
                AccountCreatedDateTime = DateTime.Now,
                LastPasswordChangeDateTime = DateTime.Now
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        return user;
    }
    public async Task<bool> PasswordChangeRequest(PasswordChangeRequestDto model, string origin)
    {
        var user = db.Users.Where(x => x.Email == model.Email).FirstOrDefault();
        if (user != null)
        {
            user.LastPasswordChangeDateTime = DateTime.Now;
            user.PasswordChangeGuid = Guid.NewGuid();
            await db.SaveChangesAsync();

            var passwordChangeUri = $"{origin}/PasswordChange/{user.PasswordChangeGuid}";
            var mailRequest = new MailRequest
            {
                From = "noreply@dsk.com",
                To = user.Email,
                Body = $"Reset your password by <a href='{passwordChangeUri}'>clicking here</a>.",
                Subject = "Password Reset"
            };
            await _mailService.SendAsync(mailRequest).ConfigureAwait(false);

            return true;
        }

        return false;
    }
    public async Task<bool> PasswordChange(PasswordChangeDto model)
    {
        var user = db.Users.Where(x => x.PasswordChangeGuid == model.PasswordChangeGuid).FirstOrDefault();
        if (user != null)
        {
            var ramdomSalt = SecurityHelpers.RandomizeSalt;

            user.HashedPassword = SecurityHelpers.HashPasword(model.Password, ramdomSalt);
            user.Salt = Convert.ToHexString(ramdomSalt);
            user.LastPasswordChangeDateTime = DateTime.Now;
            user.PasswordChangeDateTime = null;
            user.PasswordChangeGuid = null;
            await db.SaveChangesAsync();
            return true;
        };

        //todo: send email
        return false;
    }
    public async Task<bool> EmailConfirmCode(EmailConfirmCodeDto model)
    {
        var user = db.Users.Where(x => x.Salt == model.EmailConfirmCode).FirstOrDefault();
        if (user != null)
        {
            user.EmailConfirmed = true;
            await db.SaveChangesAsync();
            return true;
        };

        //todo: send email
        return false;
    }
    public async Task<APIResult<ApplicationAuthenticationProviderUserTokenDto>> RefreshToken(TokenModel model)
    {
        APIResult<ApplicationAuthenticationProviderUserTokenDto> result = new APIResult<ApplicationAuthenticationProviderUserTokenDto>();

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

        var userToken = await db.ApplicationAuthenticationProviderUserTokens.Where(x => x.RefreshToken == model.RefreshToken && x.UserId == user.Id).FirstOrDefaultAsync();

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

        var newtoken = await GenerateAuthenticationToken(user, _tokenSettings.Key);
        userToken.UserId = user.Id;
        userToken.RefreshToken = newtoken.RefreshToken;
        userToken.TokenRefreshedDateTime = DateTime.Now;
        await db.SaveChangesAsync();

        ApplicationAuthenticationProviderUserTokenDto userTokenDto = new ApplicationAuthenticationProviderUserTokenDto()
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
    private async Task<TokenModel> GenerateAuthenticationToken(User user, string key)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
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
    private async Task<User> AuthenticateUser(LoginRequestDto model, ApplicationAuthenticationProvider applicationAuthenticationProvider)
    {
        var user = await GetUserByMappedUsernameAsync(model.Email, applicationAuthenticationProvider.Id);

        bool IsUserAuthenticated = false;

        switch (applicationAuthenticationProvider.AuthenticationProviderType)
        {
            case "Active Directory":
                IsUserAuthenticated = AuthenticateUserWithDomain(model.Email, model.Password, applicationAuthenticationProvider);
                if (IsUserAuthenticated && applicationAuthenticationProvider.ActiveDirectoryFirstLoginAutoRegister)
                    user = await CreateADUserIfNotExists(model, user, applicationAuthenticationProvider);
                else
                    IsUserAuthenticated = false;
                break;
            default: //Local
                if (user != null)
                    IsUserAuthenticated = await AuthenticateUserWithLocalPassword(user, model.Password);
                break;
        }

        if (IsUserAuthenticated)
            if (user.LockoutEnabled)
                return null;
            else if (user.LockoutEnabled && user.LockoutEnd > DateTime.Now)
                return null;
            else if (user.LockoutEnabled && user.LockoutEnd <= DateTime.Now)
            {
                user.AccessFailedCount = 0;
                user.LockoutEnabled = false;
                user.LockoutEnd = null;
                await db.SaveChangesAsync();
                return user;
            }
            else
                return user;
        else
        {
            if (user != null)
            {
                user.AccessFailedCount++;
                if (user.AccessFailedCount >= 5)
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = DateTime.Now.AddMinutes(5);
                }
                await db.SaveChangesAsync();
            }
            return null;
        }

    }
    private async Task<User> CreateADUserIfNotExists(LoginRequestDto model, User? user, ApplicationAuthenticationProvider applicationAuthenticationProvider)
    {
        if (user == null)
        {
            //UserCreateDto userCreateDto = new UserCreateDto()
            //{
            //    Email = GetADUserEmail(model.Email, applicationAuthenticationProvider),
            //    Name = GetADUserDisplayName(model.Email, applicationAuthenticationProvider)
            //};

            //var result = await UserCreate(userCreateDto);

            RegisterRequestDto registerRequestDto = new RegisterRequestDto()
            {
                ADUsername = model.Email,
                Email = GetADUserEmail(model.Email, applicationAuthenticationProvider),
                Name = GetADUserDisplayName(model.Email, applicationAuthenticationProvider),
                ApplicationAuthenticationProviderGUID = applicationAuthenticationProvider.ApplicationAuthenticationProviderGuid
            };

            var result = await Register(registerRequestDto, "");

            //ApplicationAuthenticationProviderUserMappingCreateDto applicationAuthenticationProviderUserMappingCreateDto = new ApplicationAuthenticationProviderUserMappingCreateDto()
            //{
            //    Username = model.Email,
            //    UserId = result.Result.Id
            //};

            //ApplicationAuthenticationProviderUserMappingCreate(applicationAuthenticationProviderUserMappingCreateDto);
            user = await GetUserByMappedUsernameAsync(model.Email, applicationAuthenticationProvider.Id);
        }
        return user;
    }
    private async Task<bool> AuthenticateUserWithLocalPassword(User user, string password)
    {
        try
        {
            byte[] bytesalt = Convert.FromHexString(user.Salt);
            const int keySize = 64;
            const int iterations = 350000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, bytesalt, iterations, hashAlgorithm, keySize);
            return hashToCompare.SequenceEqual(Convert.FromHexString(user.HashedPassword));

        }
        catch (Exception)
        {
            return false;
        }
    }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    private bool AuthenticateUserWithDomain(string username, string password, ApplicationAuthenticationProvider applicationAuthenticationProvider)
    {
        bool isValid = false;
        try
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain,
                                applicationAuthenticationProvider.Domain,
                                applicationAuthenticationProvider.Username,
                                applicationAuthenticationProvider.Password))
                isValid = pc.ValidateCredentials(username, password);
        }
        catch (Exception ex)
        {

        }

        return isValid;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    public async Task<bool> ValidateDomainConnection(ApplicationAuthenticationProviderValidateDomainConnectionDto model)
    {
        bool isValid = false;
        try
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain,
                                model.Domain,
                                model.Username,
                                model.Password))
            {

            }

            isValid = true;
        }
        catch (Exception ex)
        {

        }

        return isValid;
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