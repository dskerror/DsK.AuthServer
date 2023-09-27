using BlazorWASMCustomAuth.Client.Services;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Components;
public partial class ApplicationRoles
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    [Parameter] public int ApplicationId { get; set; }
    private IEnumerable<ApplicationRoleDto> _pagedData;
    private MudTable<ApplicationRoleDto> _table;
    private bool _loaded;
    private int _totalItems;
    private int _currentPage;
    private string _searchString = "";
    private bool _AccessCreate;
    private bool _AccessView;


    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessView)
            _navigationManager.NavigateTo("/noaccess");
    }

    private void SetPermissions(AuthenticationState state)
    {
        _AccessView = securityService.HasPermission(state.User, Access.ApplicationRoles.View);
        _AccessCreate = securityService.HasPermission(state.User, Access.ApplicationRoles.Create);
    }

    private async Task<TableData<ApplicationRoleDto>> ServerReload(TableState state)
    {
        await LoadData(state.Page, state.PageSize, state);
        _loaded = true;
        base.StateHasChanged();
        return new TableData<ApplicationRoleDto> { TotalItems = _totalItems, Items = _pagedData };
    }

    private async Task LoadData(int pageNumber, int pageSize, TableState state)
    {
        var request = new ApplicationPagedRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = state.ToPagedRequestString(), ApplicationId = ApplicationId };
        var response = await securityService.ApplicationRolesGetAsync(request);
        if (!response.HasError)
        {
            _totalItems = response.Paging.TotalItems;
            _currentPage = response.Paging.CurrentPage;
            _pagedData = response.Result;
        }
        else
        {
            Snackbar.Add(response.Message, Severity.Error);
        }
    }

    private void OnSearch(string text)
    {
        _searchString = text;
        _table.ReloadServerData();
    }

    private async Task DeleteApplicationRole(ApplicationRoleDto context)
    {
        var parameters = new DialogParameters();
        parameters.Add("ContentText", "Are you sure you want to delete this role?");
        parameters.Add("ButtonText", "Yes");
        var dialogresult = DialogService.Show<GenericDialog>("Delete Role", parameters);
        var dialogResult = await dialogresult.Result;
        if (!dialogResult.Canceled && bool.TryParse(dialogResult.Data.ToString(), out bool resultbool))
        {
            var result = await securityService.ApplicationRoleDeleteAsync(context.Id);

            if (result != null)
                if (result.HasError)
                    Snackbar.Add(result.Message, Severity.Error);
                else
                {
                    Snackbar.Add(result.Message, Severity.Success);
                    await _table.ReloadServerData();
                }

            else
                Snackbar.Add("An Unknown Error Has Occured", Severity.Error);
        }
        else
        {
            Snackbar.Add("Operation Canceled", Severity.Warning);
        }
    }
    private async Task DisableEnableApplicationRole(ApplicationRoleDto context)
    {
        string DisableEnabledHeader = "";
        string DisableEnabledMessage = "";

        if (context.RoleDisabled == false)
        {
            DisableEnabledHeader = "Disable";
            DisableEnabledMessage = "disable";
        }
        else
        {
            DisableEnabledHeader = "Enable";
            DisableEnabledMessage = "enable";
        }

        var parameters = new DialogParameters();
        parameters.Add("ContentText", $"Are you sure you want to {DisableEnabledMessage} this role?");
        parameters.Add("ButtonText", "Yes");
        var dialogresult = DialogService.Show<GenericDialog>($"{DisableEnabledHeader} Role", parameters);
        var dialogResult = await dialogresult.Result;
        if (!dialogResult.Canceled && bool.TryParse(dialogResult.Data.ToString(), out bool resultbool))
        {
            var result = await securityService.ApplicationRoleDisableEnableAsync(context.Id);

            if (result != null)
                if (result.HasError)
                    Snackbar.Add(result.Message, Severity.Error);
                else
                {
                    Snackbar.Add(result.Message, Severity.Success);
                    await _table.ReloadServerData();
                }

            else
                Snackbar.Add("An Unknown Error Has Occured", Severity.Error);
        }
        else
        {
            Snackbar.Add("Operation Canceled", Severity.Warning);
        }
    }

    private void ViewApplicationRole(int id)
    {
        _navigationManager.NavigateTo($"/ApplicationRoleViewEdit/{ApplicationId}/{id}");
    }

    private void CreateApplicationRole()
    {
        _navigationManager.NavigateTo($"/ApplicationRoleCreate/{ApplicationId}");
    }
}
