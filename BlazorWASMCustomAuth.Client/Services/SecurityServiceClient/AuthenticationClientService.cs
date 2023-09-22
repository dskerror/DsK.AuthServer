using BlazorWASMCustomAuth.Security.Shared;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace BlazorWASMCustomAuth.Client.Services;

public partial class SecurityServiceClient
{   
    public async Task<bool> LoginAsync(UserLoginDto model)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.AuthenticationEndpoints.Login, model);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        var result = await response.Content.ReadFromJsonAsync<APIResult<TokenModel>>();
        if (result == null || result.HasError)
        {
            return false;
        }
        await _localStorageService.SetItemAsync("token", result.Result.Token);
        await _localStorageService.SetItemAsync("refreshToken", result.Result.RefreshToken);
        (_authenticationStateProvider as CustomAuthenticationStateProvider).Notify();
        return true;
    }

    public async Task<bool> LogoutAsync()
    {
        await _localStorageService.RemoveItemAsync("token");
        await _localStorageService.RemoveItemAsync("refreshToken");
        (_authenticationStateProvider as CustomAuthenticationStateProvider).Notify();
        return true;
    }
}
