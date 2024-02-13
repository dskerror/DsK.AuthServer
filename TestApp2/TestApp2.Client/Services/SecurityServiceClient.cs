using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace TestApp2.Client.Services;

public partial class SecurityServiceClient
{
    private readonly ILocalStorageService _localStorageService;
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public SecurityServiceClient(ILocalStorageService localStorageService,
        HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
    {
        _localStorageService = localStorageService;
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
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
            return string.Empty;


        if (TokenHelpers.IsTokenExpired(token))
            token = await TryRefreshToken();

        return token;
    }

    private async Task<string> TryRefreshToken()
    {
        //string token = await _localStorageService.GetItemAsync<string>("token");
        //string refreshToken = await _localStorageService.GetItemAsync<string>("refreshToken");
        //if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refreshToken))
        //{
        //    return string.Empty;
        //}

        //TokenModel tokenModel = new TokenModel(token, refreshToken);        

        //var response = await _httpClient.PostAsJsonAsync(Routes.AuthenticationEndpoints.RefreshToken, tokenModel);
        //if (!response.IsSuccessStatusCode)
        //{
        //    return string.Empty;
        //}

        //var result = await response.Content.ReadFromJsonAsync<APIResult<TokenModel>>();
        //if (result.Result == null || result.HasError == true)
        //{
        //    await _localStorageService.RemoveItemAsync("token");
        //    await _localStorageService.RemoveItemAsync("refreshToken");
        //    return string.Empty;
        //}
        //await _localStorageService.SetItemAsync("token", result.Result.Token);
        //await _localStorageService.SetItemAsync("refreshToken", result.Result.RefreshToken);

        //return result.Result.Token;

        return "";
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

    public int GetUserId(ClaimsPrincipal user)
    {
        string userId = user.Claims.Where(_ => _.Type == "UserId").Select(_ => _.Value).FirstOrDefault();
        int userIdParsed = 0;
        int.TryParse(userId, out userIdParsed);
        return userIdParsed;
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
