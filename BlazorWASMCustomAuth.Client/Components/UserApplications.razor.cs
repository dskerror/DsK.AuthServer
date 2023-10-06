using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BlazorWASMCustomAuth.Client.Services;

namespace BlazorWASMCustomAuth.Client.Components;
public partial class UserApplications
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }    
    [Parameter] public int UserId { get; set; }
    [Parameter] public EventCallback UserApplicationChanged { get; set; }
    
    private IEnumerable<ApplicationUserDto> _pagedData;
    private MudTable<ApplicationUserDto> _table;
    private int _totalItems;
    private int _currentPage;
    private string _searchString = "";
    private bool _loaded;
    private bool _AccessView;
    private bool _AccessEdit;


    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);
    }

    private void SetPermissions(AuthenticationState state)
    {
        _AccessView = securityService.HasPermission(state.User, Access.ApplicationUsers.View);
        _AccessEdit = securityService.HasPermission(state.User, Access.ApplicationUsers.Edit);
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
        var request = new ApplicationPagedRequest { Id = UserId, PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = state.ToPagedRequestString() };
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
    private async Task TogglApplicationSwitch(ChangeEventArgs e, int id)
    {   
        //var result = await securityService.UserRoleChangUseAsync(UserId, roleId, (bool)e.Value);
        //if (result != null)
        //{
        //    if (!result.HasError)
        //    {
        //        Snackbar.Add("Role Changed", Severity.Warning);
        //        await UserRoleChanged.InvokeAsync();
        //        //await LoadUserPermissions();
        //    }
        //}
    }
}
