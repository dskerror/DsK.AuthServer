using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BlazorWASMCustomAuth.Client.Services;

namespace BlazorWASMCustomAuth.Client.Components;
public partial class UserApplications
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    public List<UserApplicationGridDto> userApplications { get; set; }
    [Parameter] public int UserId { get; set; }
    [Parameter] public EventCallback UserApplicationChanged { get; set; }
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
        _AccessView = securityService.HasPermission(state.User, Access.UserApplications.View);
        _AccessEdit = securityService.HasPermission(state.User, Access.UserApplications.Edit);
    }

    private async Task Load()
    {
        var result = await securityService.UserApplicationsGetAsync(UserId);
        if (result != null)
        {
            userApplications = result.Result;
            //_loadedRolePermissionData = true;
        }
    }

    private async Task ToggleApplicationSwitch(ChangeEventArgs e, int applicationId)
    {
        var result = await securityService.UserApplicationChangeAsync(UserId, applicationId, (bool)e.Value);
        if (result != null)
        {
            if (!result.HasError)
            {
                Snackbar.Add("Application User Changed", Severity.Warning);
                await UserApplicationChanged.InvokeAsync();
                //await LoadUserPermissions();
            }
        }
    }
}
