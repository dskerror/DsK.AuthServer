using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using DsK.AuthServer.Client.Components;

namespace DsK.AuthServer.Client.Pages;
public partial class UserViewEdit
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    public UserDto model { get; set; }        
    [Parameter] public int id { get; set; }
    private bool _loaded;
    private bool _AccessView;
    private bool _AccessEdit;
    private bool _AccessUserApplicationsView;
    private bool _AccessUserPermissionsView;
    private bool _AccessUserRolesView;
    protected UserPermissions userPermissionsComponent;
    private List<BreadcrumbItem> _breadcrumbs = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Users", href: "users"),
        new BreadcrumbItem("User View/Edit", href: null, disabled: true)
    };

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessView)            
            _navigationManager.NavigateTo("NoAccess");            
        else            
            await LoadUserData();            
    }
    private void SetPermissions(AuthenticationState state)
    {
        _AccessView = securityService.HasPermission(state.User, Access.Users.View);
        _AccessEdit = securityService.HasPermission(state.User, Access.Users.Edit);
        _AccessUserApplicationsView = securityService.HasPermission(state.User, Access.UserApplications.View);
        _AccessUserPermissionsView = securityService.HasPermission(state.User, Access.UserPermissions.View);
        _AccessUserRolesView = securityService.HasPermission(state.User, Access.UserRoles.View);
    }
    private async Task LoadUserData()
    {
        var result = await securityService.UserGetAsync(id);
        if (result != null)
        {
            model = result.Result;
            _loaded = true;
        }
    }
    private async Task EditUser()
    {
        var result = await securityService.UserEditAsync(model);

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
        await LoadUserData();
    }

    private async Task RefreshUserPermissions()
    {
        await userPermissionsComponent.LoadUserPermissions();
    }
}