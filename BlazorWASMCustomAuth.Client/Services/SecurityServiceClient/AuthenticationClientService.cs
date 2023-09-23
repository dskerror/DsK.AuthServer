using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Security.Shared.ActionDtos;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace BlazorWASMCustomAuth.Client.Services;

public partial class SecurityServiceClient
{   
    public async Task<string> LoginAsync(LoginRequestDto model)
    {   
        var response = await _httpClient.PostAsJsonAsync(Routes.AuthenticationEndpoints.Login, model);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        if (result == null)
        {
            return null;
        }
        
        return result.CallbackURL;
    }

    public async Task<bool> ValidateLoginTokenAsync(string loginToken)
    {
        var model = new ValidateLoginTokenDto() { LoginToken = loginToken };
        var response = await _httpClient.PostAsJsonAsync(Routes.AuthenticationEndpoints.ValidateLoginToken, model);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        var result = await response.Content.ReadFromJsonAsync<TokenModel>();
        if (result == null)
        {
            return false;
        }        
        await _localStorageService.SetItemAsync("token", result.Token);
        await _localStorageService.SetItemAsync("refreshToken", result.RefreshToken);
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
