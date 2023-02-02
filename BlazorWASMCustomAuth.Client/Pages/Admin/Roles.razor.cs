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
    public partial class Roles
    {
        private IEnumerable<RoleDto> _pagedData;
        private MudTable<RoleDto> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        
        protected override async Task OnInitializedAsync()
        {
        }

        private async Task<TableData<RoleDto>> ServerReload(TableState state)
        {         
            await LoadData(state.Page, state.PageSize, state);            
            return new TableData<RoleDto> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedRolesRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await securityService.RolesGetPagedAsync(request);
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

        private void ViewRole(int id)
        {
            _navigationManager.NavigateTo($"/admin/roleviewedit/{id}");
        }

        private void CreateRole()
        {
            _navigationManager.NavigateTo("/admin/rolecreate");
        }
    }
}
