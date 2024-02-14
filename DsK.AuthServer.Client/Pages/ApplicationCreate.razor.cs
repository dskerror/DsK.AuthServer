using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DsK.AuthServer.Client.Pages;
public partial class ApplicationCreate
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    private ApplicationCreateDto model = new ApplicationCreateDto();
    private bool _AccessCreate;
    private List<BreadcrumbItem> _breadcrumbs = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Applications", href: "Applications"),
        new BreadcrumbItem("Application Create", href: null, disabled: true)
    };


    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessCreate)
            _navigationManager.NavigateTo("NoAccess");
    }

    private void SetPermissions(AuthenticationState state)
    {
        _AccessCreate = securityService.HasPermission(state.User, Access.Application.Create);
    }

    private async Task Create()
    {
        var result = await securityService.ApplicationCreateAsync(model);

        if (result != null)
            if (result.HasError)
                Snackbar.Add(result.Message, Severity.Error);
            else
            {
                Snackbar.Add(result.Message, Severity.Success);
                _navigationManager.NavigateTo($"Application/{result.Result.Id}");
            }
        else
            Snackbar.Add("An Unknown Error Has Occured", Severity.Error);

    }
}
