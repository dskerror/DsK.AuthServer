
using DsK.AuthServer.Client.Services;
using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace DsK.AuthServer.Client.Pages.ApplicationUsers;
public partial class ApplicationUsersIndex
{
    [Parameter] public int ApplicationId { get; set; }
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    public List<ApplicationUserGridDto> applicationsUsers { get; set; }

    private MudTable<ApplicationUserGridDto> _table;

    private IEnumerable<ApplicationUserGridDto> _pagedData;
    [Parameter] public EventCallback ApplicationUserChanged { get; set; }

    private bool _loaded;
    private int _totalItems;
    private int _currentPage;
    private string _searchString = "";

    private bool _AccessEdit;
    private bool _AccessView;


    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessView)
            _navigationManager.NavigateTo("NoAccess");
    }
    private void SetPermissions(AuthenticationState state)
    {
        _AccessView = securityService.HasPermission(state.User, Access.ApplicationUsers.View);
        _AccessEdit = securityService.HasPermission(state.User, Access.ApplicationUsers.Edit);
    }

    private async Task Load()
    {
        var result = await securityService.ApplicationUsersGetAsync(ApplicationId);
        if (result != null)
        {
            applicationsUsers = result.Result;
            //_loadedRolePermissionData = true;
        }
    }

    private async Task<TableData<ApplicationUserGridDto>> ServerReload(TableState state)
    {
        await LoadData(state.Page, state.PageSize, state);
        _loaded = true;
        base.StateHasChanged();
        return new TableData<ApplicationUserGridDto> { TotalItems = _totalItems, Items = _pagedData };
    }

    private async Task LoadData(int pageNumber, int pageSize, TableState state)
    {
        var request = new ApplicationPagedRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, OrderBy = state.ToPagedRequestString(), ApplicationId = ApplicationId };
        var response = await securityService.ApplicationUsersGetAsync(ApplicationId);
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

    private async Task ToggleApplicationSwitch(ChangeEventArgs e, int userId)
    {
        var result = await securityService.UserApplicationChangeAsync(userId, ApplicationId, (bool)e.Value);
        if (result != null)
        {
            if (!result.HasError)
            {
                Snackbar.Add("Application User Changed", Severity.Warning);
                await ApplicationUserChanged.InvokeAsync();
                //await LoadUserPermissions();
            }
        }
    }
}
