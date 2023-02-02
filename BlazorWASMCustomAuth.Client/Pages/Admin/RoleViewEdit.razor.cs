using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using BlazorWASMCustomAuth.Client.Services;
using System.Security.Claims;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages.Admin
{
    public partial class RoleViewEdit
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }

        public RoleDto role { get; set; }
        [Parameter] public int id { get; set; }
        private bool _loaded;
        private bool _EditMode;
        private List<BreadcrumbItem> _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Roles", href: "admin/roles"),
            new BreadcrumbItem("Role View/Edit", href: null, disabled: true)
        };

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var result = await securityService.RoleGet(id);
            if (result != null)
            {
                role = result.Result;
                _loaded = true;
            }
        }

        private async Task EditUser()
        {
            var result = await securityService.RoleEdit(role);
            DisableEditMode();
            if (result != null)
                if (result.HasError)
                    Snackbar.Add(result.Message, Severity.Error);
                else
                    Snackbar.Add(result.Message, Severity.Success);
            else
                Snackbar.Add("An Unknown Error Has Occured", Severity.Error);
        }

        private async Task CancelChanges()
        {
            DisableEditMode();
            Snackbar.Add("Edit canceled", Severity.Warning);
            await LoadData();
        }

        private void EnableEditMode()
        {
            _EditMode = true;
            Snackbar.Add("Edit mode enabled", Severity.Warning);
        }

        private void DisableEditMode()
        {
            _EditMode = false;
        }
    }
}
