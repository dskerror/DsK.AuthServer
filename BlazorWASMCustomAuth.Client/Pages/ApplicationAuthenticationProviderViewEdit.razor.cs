using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages;
public partial class ApplicationAuthenticationProviderViewEdit
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }

    public ApplicationAuthenticationProviderDto model { get; set; }
    [Parameter] public int ApplicationId { get; set; }
    [Parameter] public int ApplicationAuthenticationProviderId { get; set; }
    private bool _loaded;
    private bool _AccessApplicationAuthenticationProviderView;
    private bool _AccessApplicationAuthenticationProviderEdit;
    private List<BreadcrumbItem> _breadcrumbs;

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessApplicationAuthenticationProviderView)
            _navigationManager.NavigateTo("/noaccess");
        else
            await LoadData();

        _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Applications", href: "Applications"),
            new BreadcrumbItem("Applications View/Edit", href: $"ApplicationViewEdit/{ ApplicationId }"),
            new BreadcrumbItem("Application Authentication Provider View/Edit", href: null, disabled: true)
        };
    }
    private void SetPermissions(AuthenticationState state)
    {
        _AccessApplicationAuthenticationProviderView = securityService.HasPermission(state.User, Access.ApplicationAuthenticationProvider.View);
        _AccessApplicationAuthenticationProviderEdit = securityService.HasPermission(state.User, Access.ApplicationAuthenticationProvider.Edit);
    }

    private async Task LoadData()
    {
        var result = await securityService.ApplicationAuthenticationProviderGetAsync(ApplicationAuthenticationProviderId);
        if (result != null)
        {
            model = result.Result;
            _loaded = true;
        }
    }
    private async Task Edit()
    {
        model.ApplicationId = ApplicationId;
        var result = await securityService.ApplicationAuthenticationProviderEditAsync(model);

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
        await LoadData();
    }
}
