using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DsK.AuthServer.Client.Pages;
public partial class ApplicationPermissionViewEdit
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    public ApplicationPermissionUpdateDto applicationPermission { get; set; }

    [Parameter] public int ApplicationId { get; set; }
    [Parameter] public int ApplicationPermissionsId { get; set; }
    private bool _loadedPermissionData;
    private bool _loadedApplicationRolePermissionData;
    private bool _AccessView;
    private bool _AccessEdit;
    private bool _AccessApplicationRolesPermissionsView;
    private bool _AccessApplicationRolesPermissionsEdit;
    private List<BreadcrumbItem> _breadcrumbs;

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessView)
        {
            _navigationManager.NavigateTo("NoAccess");
        }
        else
        {
            await LoadPermissionData();
        }

        _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Applications", href: "Applications"),
            new BreadcrumbItem("Application View/Edit", href: $"Application/{ ApplicationId }"),
            new BreadcrumbItem("Application Permission View/Edit", href: null, disabled: true)
        };
    }

    private void SetPermissions(AuthenticationState state)
    {
        _AccessView = securityService.HasPermission(state.User, Access.ApplicationPermissions.View);
        _AccessEdit = securityService.HasPermission(state.User, Access.ApplicationPermissions.Edit);
    }

    private async Task LoadPermissionData()
    {
        var result = await securityService.ApplicationPermissionGetAsync(ApplicationPermissionsId);
        if (result != null)
        {
            applicationPermission = new ApplicationPermissionUpdateDto()
            {
                PermissionName = result.Result.PermissionName,
                PermissionDescription = result.Result.PermissionDescription
            };

        _loadedPermissionData = true;
    }
}

private async Task EditApplicationPermission()
{
    var result = await securityService.ApplicationPermissionEditAsync(applicationPermission);

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
    await LoadPermissionData();
}
}
