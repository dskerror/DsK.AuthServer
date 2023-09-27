using BlazorWASMCustomAuth.Client.Services;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Components;
public partial class ApplicationUsers
{
    [Parameter] public int ApplicationId { get; set; }
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    private IEnumerable<ApplicationUserDto> _pagedData;
    private MudTable<ApplicationUserDto> _table;
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
            _navigationManager.NavigateTo("/noaccess");
    }

    private void SetPermissions(AuthenticationState state)
    {
        _AccessView = securityService.HasPermission(state.User, Access.ApplicationUsers.View);
        _AccessCreate = securityService.HasPermission(state.User, Access.ApplicationUsers.Create);
    }

    private async Task<TableData<ApplicationUserDto>> ServerReload(TableState state)
    {
        await LoadData(state.Page, state.PageSize, state);
        _loaded = true;
        base.StateHasChanged();
        return new TableData<ApplicationUserDto> { TotalItems = _totalItems, Items = _pagedData };
    }

    private async Task LoadData(int pageNumber, int pageSize, TableState state)
    {
        var request = new ApplicationPagedRequest { ApplicationId = ApplicationId, PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = state.ToPagedRequestString() };
        var response = await securityService.ApplicationUsersGetAsync(request);
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
        //_navigationManager.NavigateTo($"/userviewedit/{id}");
    }

    private void CreateUser()
    {
        //_navigationManager.NavigateTo("/usercreate");
    }
}
