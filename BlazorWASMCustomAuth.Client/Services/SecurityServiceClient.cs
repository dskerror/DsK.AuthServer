
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
        var token = await _localStorageService.GetItemAsync<string>("token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
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
            var parsedPermissions = permissions.Value.ToString().Replace(schema,"").Trim().TrimStart('[').TrimEnd(']').Replace("\"","").Split(',');
         
                foreach (var parsedPermission in parsedPermissions)
                {
                    if(parsedPermission == permission) return true;
                }
        }
        return false;
    }
}
