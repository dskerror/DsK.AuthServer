using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages;
public partial class ApplicationAuthenticationProviderCreate
{
    [Parameter] public int ApplicationId { get; set; }
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    private ApplicationAuthenticationProviderCreateDto model = new ApplicationAuthenticationProviderCreateDto();
    private bool _AccessAuthenticationProviderCreate;
    private List<BreadcrumbItem> _breadcrumbs;

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessAuthenticationProviderCreate)
            _navigationManager.NavigateTo("/noaccess");

        _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Applications", href: "Applications"),
            new BreadcrumbItem("Applications View/Edit", href: $"ApplicationViewEdit/{ ApplicationId }"),
            new BreadcrumbItem("Application Authentication Provider Create", href: null, disabled: true)
        };
    }

    private void SetPermissions(AuthenticationState state)
    {
        _AccessAuthenticationProviderCreate = securityService.HasPermission(state.User, Access.ApplicationAuthenticationProvider.Create);
    }

    private async Task Create()
    {
        model.ApplicationId = ApplicationId;
        var result = await securityService.ApplicationAuthenticationProviderCreateAsync(model);

        if (result != null)
            if (result.HasError)
                Snackbar.Add(result.Message, Severity.Error);
            else
            {
                Snackbar.Add(result.Message, Severity.Success);
                _navigationManager.NavigateTo($"/ApplicationAuthenticationProviderViewEdit/{ApplicationId}/{result.Result.Id}");
            }
        else
            Snackbar.Add("An Unknown Error Has Occured", Severity.Error);

    }
}
