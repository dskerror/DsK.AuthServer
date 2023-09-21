using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages;
public partial class ApplicationPermissionViewEdit
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    public ApplicationPermissionUpdateDto applicationPermission { get; set; }

    [Parameter] public int ApplicationId { get; set; }
    [Parameter] public int ApplicationPermissionsId { get; set; }
    private bool _loadedPermissionData;
    private bool _loadedApplicationRolePermissionData;
    private bool _AccessApplicationPermissionView;
    private bool _AccessApplicationPermissionEdit;
    private bool _AccessApplicationRolesPermissionsView;
    private bool _AccessApplicationRolesPermissionsEdit;
    private List<BreadcrumbItem> _breadcrumbs;

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessApplicationPermissionView)
        {
            _navigationManager.NavigateTo("/noaccess");
        }
        else
        {
            await LoadPermissionData();
        }

        _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Applications", href: "Applications"),
            new BreadcrumbItem("Applications View/Edit", href: $"ApplicationViewEdit/{ ApplicationId }"),
            new BreadcrumbItem("Application Permission View/Edit", href: null, disabled: true)
        };
    }

    private void SetPermissions(AuthenticationState state)
    {
        _AccessApplicationPermissionView = securityService.HasPermission(state.User, Access.ApplicationPermissions.View);
        _AccessApplicationPermissionEdit = securityService.HasPermission(state.User, Access.ApplicationPermissions.Edit);
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
