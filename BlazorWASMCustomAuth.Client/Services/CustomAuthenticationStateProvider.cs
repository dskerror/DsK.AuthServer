using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;
using System.Linq.Dynamic.Core.Tokenizer;

namespace BlazorWASMCustomAuth.Client.Services;
public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;

    public CustomAuthenticationStateProvider(ILocalStorageService localStorageService,
        HttpClient httpClient)
    {
        _localStorageService = localStorageService;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string token = await _localStorageService.GetItemAsync<string>("token");        
        if (string.IsNullOrEmpty(token) || TokenHelpers.IsTokenExpired(token))
        {
            var anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity() { }));
            return anonymous;
        }
        var userClaimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(TokenHelpers.ParseClaimsFromJwt(token), "Fake Authentication"));

        var loginUser = new AuthenticationState(userClaimPrincipal);
        return loginUser;
    }

    public void Notify()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
