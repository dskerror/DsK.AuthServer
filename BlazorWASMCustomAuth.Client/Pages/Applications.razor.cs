using BlazorWASMCustomAuth.Client.Components;
using BlazorWASMCustomAuth.Client.Services;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Security.Shared.Constants;
using BlazorWASMCustomAuth.Security.Shared.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages;

public partial class Applications
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    private IEnumerable<ApplicationDto> _pagedData;
    private MudTable<ApplicationDto> _table;
    private int _totalItems;
    private int _currentPage;
    private string _searchString = "";
    private bool _loaded;
    private bool _AccessApplicationView;
    private bool _AccessApplicationCreate;

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessApplicationView)
            _navigationManager.NavigateTo("/noaccess");
    }

    private void SetPermissions(AuthenticationState state)
    {
        _AccessApplicationView = securityService.HasPermission(state.User, Access.Application.View);
        _AccessApplicationCreate = securityService.HasPermission(state.User, Access.Application.Create);
    }
    private async Task<TableData<ApplicationDto>> ServerReload(TableState state)
    {
        await LoadData(state.Page, state.PageSize, state);
        _loaded = true;
        base.StateHasChanged();
        return new TableData<ApplicationDto> { TotalItems = _totalItems, Items = _pagedData };
    }

    private async Task LoadData(int pageNumber, int pageSize, TableState state)
    {
        var request = new PagedRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = state.ToPagedRequestString() };
        var response = await securityService.ApplicationsGetAsync(request);
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

    private void ViewApplication(int id)
    {
        _navigationManager.NavigateTo($"/ApplicationViewEdit/{id}");
    }

    private async Task DeleteApplication(ApplicationDto context)
    {
        var parameters = new DialogParameters();
        parameters.Add("ContentText", "Are you sure you want to delete this application?");
        parameters.Add("ButtonText", "Yes");
        var dialogresult = DialogService.Show<DeleteApplicationDialog>("Delete Application", parameters);
        var dialogResult = await dialogresult.Result;
        if (!dialogResult.Canceled && bool.TryParse(dialogResult.Data.ToString(), out bool resultbool))
        {
            var result = await securityService.ApplicationDeleteAsync(context.Id);

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
            Snackbar.Add("Delete Canceled", Severity.Warning);
        }
    }

    private void CreateApplication()
    {
        _navigationManager.NavigateTo("/ApplicationCreate");
    }
}
