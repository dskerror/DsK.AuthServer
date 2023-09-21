using BlazorWASMCustomAuth.Client.Services;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages;
public partial class Users
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    private IEnumerable<UserDto> _pagedData;
    private MudTable<UserDto> _table;
    private bool _loaded;
    private int _totalItems;
    private int _currentPage;
    private string _searchString = "";
    private bool _AccessUsersView;
    private bool _AccessUsersCreate;

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessUsersView)
            _navigationManager.NavigateTo("/noaccess");
    }

    private void SetPermissions(AuthenticationState state)
    {
        _AccessUsersView = securityService.HasPermission(state.User, Access.Users.View);
        _AccessUsersCreate = securityService.HasPermission(state.User, Access.Users.Create);
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
        var request = new PagedRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = state.ToPagedRequestString() };
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
        _navigationManager.NavigateTo($"/userviewedit/{id}");
    }

    private void CreateUser()
    {
        _navigationManager.NavigateTo("/usercreate");
    }
}
