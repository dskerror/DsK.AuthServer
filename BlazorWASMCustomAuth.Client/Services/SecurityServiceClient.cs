
using Blazored.LocalStorage;
using BlazorWASMCustomAuth.Client.Security;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Headers;
using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Client.Services.Requests;
using System.Security.Claims;
using BlazorWASMCustomAuth.Security.Shared.Constants;

namespace BlazorWASMCustomAuth.Client.Services;

public partial class SecurityServiceClient
{
    private readonly AuthenticationStateProvider _customAuthenticationProvider;
    private readonly ILocalStorageService _localStorageService;
    private readonly HttpClient _httpClient;
    public SecurityServiceClient(ILocalStorageService localStorageService,
        AuthenticationStateProvider customAuthenticationProvider,
        HttpClient httpClient)
    {
        _localStorageService = localStorageService;
        _customAuthenticationProvider = customAuthenticationProvider;
        _httpClient = httpClient;
    }
    private async Task PrepareBearerToken()
    {
        var token = await GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
    }

    public async Task<string> GetTokenAsync()
    {
        string token = await _localStorageService.GetItemAsync<string>("token");
        if (string.IsNullOrEmpty(token))
        {
            return string.Empty;
        }

        if (ValidateTokenExpiration(token))
        {
            return token;
        }

        string refreshToken = await _localStorageService.GetItemAsync<string>("refreshToken");
        if (string.IsNullOrEmpty(refreshToken))
        {
            return string.Empty;
        }

        TokenModel tokenModel = new TokenModel(token, refreshToken);
        return await RefreshTokenEndPoint(tokenModel);
    }

    private bool ValidateTokenExpiration(string token)
    {
        List<Claim> claims = JwtParser.ParseClaimsFromJwt(token).ToList();
        if (claims?.Count == 0)
        {
            return false;
        }
        string expirationSeconds = claims.Where(_ => _.Type.ToLower() == "exp").Select(_ => _.Value).FirstOrDefault();
        if (string.IsNullOrEmpty(expirationSeconds))
        {
            return false;
        }

        var expirationDate = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(expirationSeconds));
        if (expirationDate < DateTime.UtcNow)
        {
            return false;
        }
        return true;
    }

    private async Task<string> RefreshTokenEndPoint(TokenModel tokenModel)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.AuthenticationEndpoints.RefreshToken, tokenModel);
        if (!response.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var result = await response.Content.ReadFromJsonAsync<APIResult<TokenModel>>();
        if (result == null)
        {
            return string.Empty; ;
        }
        await _localStorageService.SetItemAsync("token", result.Result.Token);
        await _localStorageService.SetItemAsync("refreshToken", result.Result.RefreshToken);
        ((CustomAuthenticationProvider)_customAuthenticationProvider).Notify();

        return result.Result.Token;
    }


    public bool HasPermission(ClaimsPrincipal user, string permission)
    {
        //ClaimsPrincipal newuser = user;
        // && (x.Value.Contains(permission)|| x.Value.Contains("Admin")
        var permissions = user.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault();

        if (UserHasPermission(permissions, permission) || UserHasPermission(permissions, "Admin"))
            return true;
        else
            return false;
    }

    private bool UserHasPermission(Claim permissions, string permission)
    {
        if (permissions != null)
        {
            var schema = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role : ";
            var parsedPermissions = permissions.Value.ToString().Replace(schema, "").Trim().TrimStart('[').TrimEnd(']').Replace("\"", "").Split(',');

            foreach (var parsedPermission in parsedPermissions)
            {
                if (parsedPermission == permission) return true;
            }
        }
        return false;
    }
}
