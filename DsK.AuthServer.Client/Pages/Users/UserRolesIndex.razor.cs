using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DsK.AuthServer.Client.Pages.Users;
public partial class UserRolesIndex
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    public List<UserRoleGridDto> userRoles { get; set; }
    [Parameter] public int UserId { get; set; }
    [Parameter] public EventCallback UserRoleChanged { get; set; }
    private bool _AccessEdit;
    private bool _AccessView;


    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (_AccessView)
            await LoadUserRoles();
    }
    private void SetPermissions(AuthenticationState state)
    {
        _AccessView = securityService.HasPermission(state.User, Access.UserRoles.View);
        _AccessEdit = securityService.HasPermission(state.User, Access.UserRoles.Edit);
    }

    private async Task LoadUserRoles()
    {
        var result = await securityService.UserRolesGetAsync(UserId);
        if (result != null)
        {
            userRoles = result.Result;                
            //_loadedRolePermissionData = true;
        }
    }

    private async Task ToggleRoleSwitch(ChangeEventArgs e, int roleId)
    {   
        var result = await securityService.UserRoleChangeAsync(UserId, roleId, (bool)e.Value);
        if (result != null)
        {
            if (!result.HasError)
            {
                Snackbar.Add("Role Changed", Severity.Warning);
                await UserRoleChanged.InvokeAsync();
                //await LoadUserPermissions();
            }
        }
    }
}
