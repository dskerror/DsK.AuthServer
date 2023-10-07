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
    private bool _AccessCreate;
    private List<BreadcrumbItem> _breadcrumbs;

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessCreate)
            _navigationManager.NavigateTo("/noaccess");

        _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Applications", href: "Applications"),
            new BreadcrumbItem("Application View/Edit", href: $"Application/{ ApplicationId }"),
            new BreadcrumbItem("Application Authentication Provider Create", href: null, disabled: true)
        };
    }

    private void SetPermissions(AuthenticationState state)
    {
        _AccessCreate = securityService.HasPermission(state.User, Access.ApplicationAuthenticationProvider.Create);
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

    private async Task ValidateConnection()
    {
        var IsValid = await securityService.ValidateDomainConnectionAsync(model.Domain, model.Username, model.Password);

        if (IsValid)
            Snackbar.Add("Connection is valid.", Severity.Success);
        else
            Snackbar.Add("Connection is not valid.", Severity.Error);
    }
}
