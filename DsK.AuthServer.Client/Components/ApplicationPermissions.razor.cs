using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using DsK.AuthServer.Client.Services;

namespace DsK.AuthServer.Client.Components;
public partial class ApplicationPermissions
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    public List<ApplicationPermissionGridDto> applicationPermissions { get; set; }
    [Parameter] public int ApplicationId { get; set; }

    private MudTable<ApplicationPermissionDto> _table;
    private IEnumerable<ApplicationPermissionDto> _pagedData;
    private bool _loaded;
    private int _totalItems;
    private int _currentPage;
    private string _searchString = "";
    private bool _AccessView;    
    private bool _AccessCreate;

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessView)
            _navigationManager.NavigateTo("NoAccess");
    }
    private void SetPermissions(AuthenticationState state)
    {
        _AccessView = securityService.HasPermission(state.User, Access.ApplicationPermissions.View);        
        _AccessCreate = securityService.HasPermission(state.User, Access.ApplicationPermissions.Create);
    }

    private async Task LoadData(int pageNumber, int pageSize, TableState state)
    {
        var request = new ApplicationPagedRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = state.ToPagedRequestString(), ApplicationId = ApplicationId };
        var response = await securityService.ApplicationPermissionsGetAsync(request);
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

    private void CreateApplicationPermission()
    {
        _navigationManager.NavigateTo($"Application/Permission/Create/{ApplicationId}");
    }

    private void ViewApplicationPermission(int id)
    {
        _navigationManager.NavigateTo($"Application/Permission/{ApplicationId}/{id}");
    }

    private async Task<TableData<ApplicationPermissionDto>> ServerReload(TableState state)
    {
        await LoadData(state.Page, state.PageSize, state);
        _loaded = true;
        base.StateHasChanged();
        return new TableData<ApplicationPermissionDto> { TotalItems = _totalItems, Items = _pagedData };
    }


    private void OnSearch(string text)
    {
        _searchString = text;
        _table.ReloadServerData();
    }

    private async Task DeleteApplicationPermission(ApplicationPermissionDto context)
    {
        var parameters = new DialogParameters();
        parameters.Add("ContentText", "Are you sure you want to delete this permission?");
        parameters.Add("ButtonText", "Yes");
        var dialogresult = DialogService.Show<GenericDialog>("Delete Permission", parameters);
        var dialogResult = await dialogresult.Result;
        if (!dialogResult.Canceled && bool.TryParse(dialogResult.Data.ToString(), out bool resultbool))
        {
            var result = await securityService.ApplicationPermissionDeleteAsync(context.Id);

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
    private async Task IsEnabledToggle(ApplicationPermissionDto context)
    {
        string IsEnabledToggleHeader = "";
        string IsEnabledToggleMessage = "";

        if (context.IsEnabled)
        {
            IsEnabledToggleHeader = "Disable";
            IsEnabledToggleMessage = "disable";
        }
        else
        {
            IsEnabledToggleHeader = "Enable";
            IsEnabledToggleMessage = "enable";
        }

        var parameters = new DialogParameters();
        parameters.Add("ContentText", $"Are you sure you want to {IsEnabledToggleMessage} this permission?");
        parameters.Add("ButtonText", "Yes");
        var dialogresult = DialogService.Show<GenericDialog>($"{IsEnabledToggleHeader} Permission", parameters);
        var dialogResult = await dialogresult.Result;
        if (!dialogResult.Canceled && bool.TryParse(dialogResult.Data.ToString(), out bool resultbool))
        {
            var result = await securityService.ApplicationPermissionDisableEnableAsync(context.Id);

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
}
