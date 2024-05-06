using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using DsK.AuthServer.Security.Shared;

namespace DsK.AuthServer.Client.Pages.ApplicationRoles;
public partial class ApplicationRoleCreate
{
    [Parameter] public int ApplicationId { get; set; }
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    private ApplicationRoleCreateDto model = new ApplicationRoleCreateDto();
    private bool _AccessCreate;
    private List<BreadcrumbItem> _breadcrumbs;

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessCreate)
            _navigationManager.NavigateTo("NoAccess");

        _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Applications", href: "Applications"),
            new BreadcrumbItem("Application View/Edit", href: $"Application/{ ApplicationId }"),
            new BreadcrumbItem("Application Role Create", href: null, disabled: true)
        };
    }

    private void SetPermissions(AuthenticationState state)
    {
        _AccessCreate = securityService.HasPermission(state.User, Access.ApplicationRoles.Create);
    }

    private async Task Create()
    {
        model.ApplicationId = ApplicationId;
        var result = await securityService.ApplicationRoleCreateAsync(model);

        if (result != null)
            if (result.HasError)
                Snackbar.Add(result.Message, Severity.Error);
            else
            {
                Snackbar.Add(result.Message, Severity.Success);
                _navigationManager.NavigateTo($"Application/Role/{ApplicationId}/{result.Result.Id}");
            }
        else
            Snackbar.Add("An Unknown Error Has Occured", Severity.Error);

    }
}
