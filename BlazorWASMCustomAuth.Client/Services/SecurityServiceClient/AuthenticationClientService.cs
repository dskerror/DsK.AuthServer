using BlazorWASMCustomAuth.Security.Shared;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace BlazorWASMCustomAuth.Client.Services;

public partial class SecurityServiceClient
{   
    public async Task<TokenModel> LoginAsync(UserLoginDto model)
    {
        TokenModel tokenModel = new TokenModel("","");
        var response = await _httpClient.PostAsJsonAsync(Routes.AuthenticationEndpoints.Login, model);
        if (!response.IsSuccessStatusCode)
        {
            return tokenModel;
        }
        var result = await response.Content.ReadFromJsonAsync<APIResult<TokenModel>>();
        if (result == null || result.HasError)
        {
            return tokenModel;
        }
        tokenModel = result.Result;
        await _localStorageService.SetItemAsync("token", result.Result.Token);
        await _localStorageService.SetItemAsync("refreshToken", result.Result.RefreshToken);
        (_authenticationStateProvider as CustomAuthenticationStateProvider).Notify();
        return tokenModel;
    }

    public async Task<bool> LogoutAsync()
    {
        await _localStorageService.RemoveItemAsync("token");
        await _localStorageService.RemoveItemAsync("refreshToken");
        (_authenticationStateProvider as CustomAuthenticationStateProvider).Notify();
        return true;
    }
}
