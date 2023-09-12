using BlazorWASMCustomAuth.Client.Services;
using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Security.Shared.Constants;
using BlazorWASMCustomAuth.Security.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Net;
using System.Security.Claims;

namespace BlazorWASMCustomAuth.Client.Pages
{
    public partial class AuthenticationProviders
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
        private IEnumerable<AuthenticationProviderDto> _pagedData;
        private MudTable<AuthenticationProviderDto> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _loaded;
        private bool _AccessAuthenticationProviderView;
        private bool _AccessAuthenticationProviderCreate;

        protected override async Task OnInitializedAsync()
        {
            var state = await authenticationState;
            SetPermissions(state);

            if (!_AccessAuthenticationProviderView)
                _navigationManager.NavigateTo("/noaccess");
        }

        private void SetPermissions(AuthenticationState state)
        {
            _AccessAuthenticationProviderView = securityService.HasPermission(state.User, Access.AuthenticationProvider.View);
            _AccessAuthenticationProviderCreate = securityService.HasPermission(state.User, Access.AuthenticationProvider.Create);
        }
        private async Task<TableData<AuthenticationProviderDto>> ServerReload(TableState state)
        {         
            await LoadData(state.Page, state.PageSize, state);
            _loaded = true;
            base.StateHasChanged();
            return new TableData<AuthenticationProviderDto> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            var request = new PagedRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = state.ToPagedRequestString() };
            var response = await securityService.AuthenticationProvidersGetAsync(request);
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
}
