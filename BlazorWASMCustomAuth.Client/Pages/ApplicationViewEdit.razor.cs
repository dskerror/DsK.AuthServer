using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages;
public partial class ApplicationViewEdit
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }

    public ApplicationDto model { get; set; }
    [Parameter] public int id { get; set; }
    private bool _loaded;
    private bool _AccessView;
    private bool _AccessEdit;
    private List<BreadcrumbItem> _breadcrumbs = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Applications", href: "Applications"),
        new BreadcrumbItem("Application View/Edit", href: null, disabled: true)
    };

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessView)
            _navigationManager.NavigateTo("/NoAccess");
        else
            await LoadData();
    }

    private void SetPermissions(AuthenticationState state)
    {
        _AccessView = securityService.HasPermission(state.User, Access.Application.View);
        _AccessEdit = securityService.HasPermission(state.User, Access.Application.Edit);
    }

    private async Task LoadData()
    {
        var result = await securityService.ApplicationGetAsync(id);
        if (result != null)
        {
            model = result.Result;
            _loaded = true;
        }
    }

    private async Task Edit()
    {
        ApplicationUpdateDto applicationUpdateDto = new ApplicationUpdateDto() { Id = model.Id, ApplicationDesc = model.ApplicationDesc, IsEnabled = model.IsEnabled };
        var result = await securityService.ApplicationEditAsync(applicationUpdateDto);

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

    private async Task GenerateNewAPIKey(ApplicationDto model)
    {
        var result = await securityService.ApplicationGenerateNewAPIKeyAsync(model);

        if (result != null)
            if (result.HasError)
                Snackbar.Add(result.Message, Severity.Error);
            else
                Snackbar.Add("New API Key Generated", Severity.Success);
        else
            Snackbar.Add("An Unknown Error Has Occured", Severity.Error);

        await LoadData();

    }
}
