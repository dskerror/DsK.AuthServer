using DsK.AuthServer.Security.Shared;
using DsK.AuthServer.Security.Shared.ActionDtos;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace DsK.AuthServer.Client.Services;

public partial class SecurityServiceClient
{   
    public async Task<APIResponse<LoginResponseDto>> LoginAsync(LoginRequestDto model)
    {   
        var response = await _httpClient.PostAsJsonAsync(Routes.AuthenticationEndpoints.Login, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResponse<LoginResponseDto>>();        

        return result;
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
    public async Task<APIResponse<string>> RegisterAsync(RegisterRequestDto model)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.AuthenticationEndpoints.Register, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResponse<string>>();
        return result;
    }
    public async Task<bool> PasswordChangeRequestAsync(PasswordChangeRequestDto model)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.AuthenticationEndpoints.PasswordChangeRequest, model);
        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<bool>();
        return result;
    }
    public async Task<bool> PasswordChangeAsync(PasswordChangeDto model)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.AuthenticationEndpoints.PasswordChange, model);
        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<bool>();
        return result;
    }

    public async Task<bool> EmailConfirmAsync(EmailConfirmCodeDto model)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.AuthenticationEndpoints.EmailConfirm, model);
        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<bool>();
        return result;
    }
}
