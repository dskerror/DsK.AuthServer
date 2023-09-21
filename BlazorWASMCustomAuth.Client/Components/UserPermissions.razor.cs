using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Components;
public partial class UserPermissions
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    public List<UserPermissionGridDto> userPermissions { get; set; }
    [Parameter] public int UserId { get; set; }

    private bool _AccessUserPermissionsView;
    private bool _AccessUserPermissionsEdit;

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);
        
        if (_AccessUserPermissionsView)
            await LoadUserPermissions();
    }
    private void SetPermissions(AuthenticationState state)
    {
        _AccessUserPermissionsView = securityService.HasPermission(state.User, Access.UserPermissions.View);
        _AccessUserPermissionsEdit = securityService.HasPermission(state.User, Access.UserPermissions.Edit);
    }
    public async Task LoadUserPermissions()
    {
        var result = await securityService.UserPermissionsGetAsync(UserId);
        if (result != null)
        {
            userPermissions = result.Result;
        }
        StateHasChanged();
    }

    private async Task TogglePermissionEnabledSwitch(ChangeEventArgs e, int permissionId, bool allow)
    {
        UserPermissionChangeDto userPermissionChangeDto = new UserPermissionChangeDto()
        {
            UserId = UserId,
            PermissionId = permissionId,
            Enabled = (bool)e.Value,
            Allow = allow
        };
        var result = await securityService.UserPermissionChangeAsync(userPermissionChangeDto);
        if (result != null)
        {
            if (!result.HasError)
            {
                Snackbar.Add("Permissions Changed", Severity.Warning);
            }
        }
    }
    private async Task TogglePermissionAllowSwitch(ChangeEventArgs e, int permissionId)
    {
        UserPermissionChangeDto userPermissionChangeDto = new UserPermissionChangeDto()
        {
            UserId = UserId,
            PermissionId = permissionId,
            Enabled = true,
            Allow = (bool)e.Value
        };

        var result = await securityService.UserPermissionChangeAsync(userPermissionChangeDto);
        if (result != null)
        {
            if (!result.HasError)
            {
                Snackbar.Add("Permissions Changed", Severity.Warning);
            }
        }
    }
}
