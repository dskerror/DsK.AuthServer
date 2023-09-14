using BlazorWASMCustomAuth.Client.Services;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Security.Shared.Constants;
using BlazorWASMCustomAuth.Security.Shared.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Policy;

namespace BlazorWASMCustomAuth.Client.Components
{
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
        private bool _AccessApplicationRolesCreate;
        private bool _AccessApplicationRolesView;


        protected override async Task OnInitializedAsync()
        {
            var state = await authenticationState;
            SetPermissions(state);

            if (!_AccessApplicationRolesView)
                _navigationManager.NavigateTo("/noaccess");
        }

        private void SetPermissions(AuthenticationState state)
        {
            _AccessApplicationRolesView = securityService.HasPermission(state.User, Access.ApplicationRoles.View);
            _AccessApplicationRolesCreate = securityService.HasPermission(state.User, Access.ApplicationRoles.Create);
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

        private void ViewApplicationRole(int id)
        {
            _navigationManager.NavigateTo($"/ApplicationRoleViewEdit/{ApplicationId}/{id}");
        }

        private void CreateApplicationRole()
        {
            _navigationManager.NavigateTo($"/ApplicationRoleCreate/{ApplicationId}");
        }
    }
}
