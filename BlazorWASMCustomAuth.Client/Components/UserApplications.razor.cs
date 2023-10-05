using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Components;
public partial class UserApplications
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    public List<ApplicationUsers> model { get; set; }
    [Parameter] public int UserId { get; set; }
    [Parameter] public EventCallback UserRoleChanged { get; set; }
    private bool _AccessEdit;
    private bool _AccessView;


    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (_AccessView)
            await Load();
    }
    private void SetPermissions(AuthenticationState state)
    {
        _AccessView = securityService.HasPermission(state.User, Access.ApplicationUsers.View);
        _AccessEdit = securityService.HasPermission(state.User, Access.ApplicationUsers.Edit);
    }

    private async Task Load()
    {
        var result = await securityService.ApplicationUsersGetAsync(UserId);
        if (result != null)
        {
            model = result.Result;                
            //_loadedRolePermissionData = true;
        }
    }

    private async Task ToggleRoleSwitch(ChangeEventArgs e, int roleId)
    {   
        //var result = await securityService.UserRoleChangUseAsync(UserId, roleId, (bool)e.Value);
        //if (result != null)
        //{
        //    if (!result.HasError)
        //    {
        //        Snackbar.Add("Role Changed", Severity.Warning);
        //        await UserRoleChanged.InvokeAsync();
        //        //await LoadUserPermissions();
        //    }
        //}
    }
}
