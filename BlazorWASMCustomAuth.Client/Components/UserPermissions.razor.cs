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

    private bool _AccessView;
    private bool _AccessEdit;

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);
        
        if (_AccessView)
            await LoadUserPermissions();
    }
    private void SetPermissions(AuthenticationState state)
    {
        _AccessView = securityService.HasPermission(state.User, Access.UserPermissions.View);
        _AccessEdit = securityService.HasPermission(state.User, Access.UserPermissions.Edit);
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

    private async Task TogglePermissionEnabledSwitch(ChangeEventArgs e, int permissionId, bool overwrite)
    {
        UserPermissionChangeDto userPermissionChangeDto = new UserPermissionChangeDto()
        {
            UserId = UserId,
            PermissionId = permissionId,
            IsEnabled = (bool)e.Value,
            Overwrite = overwrite
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
            IsEnabled = true,
            Overwrite = (bool)e.Value
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
