using DsK.AuthServer.Client.Services;
using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace DsK.AuthServer.Client.Pages.Users;
public partial class UsersIndex
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    private IEnumerable<UserDto> _pagedData;
    private MudTable<UserDto> _table;
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
        _AccessView = securityService.HasPermission(state.User, Access.Users.View);
        _AccessCreate = securityService.HasPermission(state.User, Access.Users.Create);
    }

    private async Task<TableData<UserDto>> ServerReload(TableState state)
    {
        await LoadData(state.Page, state.PageSize, state);
        _loaded = true;
        base.StateHasChanged();
        return new TableData<UserDto> { TotalItems = _totalItems, Items = _pagedData };
    }

    private async Task LoadData(int pageNumber, int pageSize, TableState state)
    {
        var request = new PagedRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, OrderBy = state.ToPagedRequestString() };
        var response = await securityService.UsersGetAsync(request);
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

    private void ViewUser(int id)
    {
        _navigationManager.NavigateTo($"User/{id}");
    }

    private void CreateUser()
    {
        _navigationManager.NavigateTo("User/Create");
    }
}
