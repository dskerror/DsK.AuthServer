
using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace DsK.AuthServer.Client.Pages.ApplicationUsers;
public partial class ApplicationUsersIndex
{
    [Parameter] public int ApplicationId { get; set; }
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    public List<ApplicationUserGridDto> applicationsUsers { get; set; }

    [Parameter] public EventCallback ApplicationUserChanged { get; set; }
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
        var result = await securityService.ApplicationUsersGetAsync(ApplicationId);
        if (result != null)
        {
            applicationsUsers = result.Result;
            //_loadedRolePermissionData = true;
        }
    }

    private async Task ToggleApplicationSwitch(ChangeEventArgs e, int userId)
    {
        var result = await securityService.UserApplicationChangeAsync(userId, ApplicationId, (bool)e.Value);
        if (result != null)
        {
            if (!result.HasError)
            {
                Snackbar.Add("Application User Changed", Severity.Warning);
                await ApplicationUserChanged.InvokeAsync();
                //await LoadUserPermissions();
            }
        }
    }
}
