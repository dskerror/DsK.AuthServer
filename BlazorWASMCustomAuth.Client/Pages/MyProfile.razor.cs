using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BlazorWASMCustomAuth.Client.Components;

namespace BlazorWASMCustomAuth.Client.Pages;
public partial class MyProfile
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    public UserDto user { get; set; }
    
    private bool _loaded;
    protected UserPermissions userPermissionsComponent;
    private List<BreadcrumbItem> _breadcrumbs = new List<BreadcrumbItem>
    {        
        new BreadcrumbItem("My Profile", href: null, disabled: true)
    };

    protected override async Task OnInitializedAsync()
    {
        int userId = await GetUsedIdFromAuthenticationState();
        await LoadUserData(userId);
    }

    private async Task<int> GetUsedIdFromAuthenticationState()
    {
        var state = await authenticationState;
        var userIdString = state.User.Claims.Where(_ => _.Type == "UserId").Select(_ => _.Value).FirstOrDefault();
        int userId = int.Parse(userIdString);
        return userId;
    }

    private async Task LoadUserData(int userId)
    {
        
        var result = await securityService.UserGetAsync(userId);
        if (result != null)
        {
            user = result.Result;
            _loaded = true;
        }
    }
    private async Task EditUser()
    {
        var result = await securityService.UserEditAsync(user);

        if (result != null)
            if (result.HasError)
                Snackbar.Add(result.Message, Severity.Error);
            else
                Snackbar.Add(result.Message, Severity.Success);
        else
            Snackbar.Add("An Unknown Error Has Occured", Severity.Error);
    }
    private async Task CancelChanges()
    {
        Snackbar.Add("Edit canceled", Severity.Warning);

        int userId = await GetUsedIdFromAuthenticationState();
        await LoadUserData(userId);
    }

    private async Task RefreshUserPermissions()
    {
        await userPermissionsComponent.LoadUserPermissions();
    }
}