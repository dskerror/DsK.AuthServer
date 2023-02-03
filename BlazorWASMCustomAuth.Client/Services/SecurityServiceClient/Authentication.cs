
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

namespace BlazorWASMCustomAuth.Client.Services;

public partial class SecurityServiceClient
{   
    public async Task<bool> LoginAsync(UserLoginDto model)
    {
        var response = await _httpClient.PostAsJsonAsync<UserLoginDto>(Routes.AuthenticationEndpoints.Login, model);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        var result = await response.Content.ReadFromJsonAsync<APIResult<TokenModel>>();
        if (result == null)
        {
            return false;
        }
        await _localStorageService.SetItemAsync("token", result.Result.Token);
        await _localStorageService.SetItemAsync("refreshToken", result.Result.RefreshToken);
        ((CustomAuthenticationProvider)_customAuthenticationProvider).Notify();
        return true;
    }

    public async Task<bool> LogoutAsync()
    {
        await _localStorageService.RemoveItemAsync("token");
        await _localStorageService.RemoveItemAsync("refreshToken");
        ((CustomAuthenticationProvider)_customAuthenticationProvider).Notify();
        return true;
    }
}
