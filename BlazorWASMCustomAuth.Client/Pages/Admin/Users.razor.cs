using BlazorWASMCustomAuth.Client.Services;
using BlazorWASMCustomAuth.Client.Services.Requests;
using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Security.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Net;
using System.Security.Claims;

namespace BlazorWASMCustomAuth.Client.Pages.Admin
{
    public partial class Users
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
        private IEnumerable<UserDto> _pagedData;
        private MudTable<UserDto> _table;
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
            return new TableData<UserDto> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new PagedRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
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
            _navigationManager.NavigateTo($"/admin/userviewedit/{id}");
        }

        private void CreateUser()
        {
            _navigationManager.NavigateTo("/admin/usercreate");
        }
    }
}
