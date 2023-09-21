using BlazorWASMCustomAuth.Client.Services;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Components;
public partial class ApplicationAuthenticationProviders
{
    [Parameter] public int ApplicationId { get; set; }
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    private IEnumerable<ApplicationAuthenticationProviderDto> _pagedData;
    private MudTable<ApplicationAuthenticationProviderDto> _table;
    private int _totalItems;
    private int _currentPage;
    private string _searchString = "";
    private bool _loaded;
    private bool _AccessApplicationAuthenticationProviderView;
    private bool _AccessApplicationAuthenticationProviderCreate;

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessApplicationAuthenticationProviderView)
            _navigationManager.NavigateTo("/noaccess");
    }

    private void SetPermissions(AuthenticationState state)
    {
        _AccessApplicationAuthenticationProviderView = securityService.HasPermission(state.User, Access.AuthenticationProvider.View);
        _AccessApplicationAuthenticationProviderCreate = securityService.HasPermission(state.User, Access.AuthenticationProvider.Create);
    }
    private async Task<TableData<ApplicationAuthenticationProviderDto>> ServerReload(TableState state)
    {
        await LoadData(state.Page, state.PageSize, state);
        _loaded = true;
        StateHasChanged();
        return new TableData<ApplicationAuthenticationProviderDto> { TotalItems = _totalItems, Items = _pagedData };
    }

    private async Task LoadData(int pageNumber, int pageSize, TableState state)
    {
        var request = new ApplicationPagedRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = state.ToPagedRequestString(), ApplicationId = ApplicationId };
        var response = await securityService.ApplicationAuthenticationProvidersGetAsync(request);
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

    private void ViewAuthenticationProvider(int id)
    {
        _navigationManager.NavigateTo($"/authenticationProviderViewEdit/{id}");
    }

    private void CreateAuthenticationProvider()
    {
        _navigationManager.NavigateTo("/authenticationProviderCreate");
    }
}
