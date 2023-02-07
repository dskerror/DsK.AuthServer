using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using BlazorWASMCustomAuth.Client.Services;
using System.Security.Claims;
using MudBlazor;
using System.Diagnostics;

namespace BlazorWASMCustomAuth.Client.Pages.Admin
{
    public partial class RoleViewEdit
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }

        public RoleDto role { get; set; }
        public List<RolePermissionGridDto> rolePermissions { get; set; }
        [Parameter] public int id { get; set; }
        private bool _loadedPermissionData;
        private bool _loadedRolePermissionData;
        private bool _EditMode;
        
        private List<BreadcrumbItem> _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Roles", href: "admin/roles"),
            new BreadcrumbItem("Role View/Edit", href: null, disabled: true)
        };

        protected override async Task OnInitializedAsync()
        {
            await LoadPermissionData();
            await LoadRolePermissionData();
        }

        private async Task LoadPermissionData()
        {
            var result = await securityService.RoleGetAsync(id);
            if (result != null)
            {
                role = result.Result;
                _loadedPermissionData = true;
            }
        }

        private async Task LoadRolePermissionData()
        {
            var result = await securityService.RolePermissionsGetAsync(id);
            if (result != null)
            {
                rolePermissions = result.Result;
                _loadedRolePermissionData = true;
            }
        }

        private async Task EditRole()
        {
            var result = await securityService.RoleEditAsync(role);
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
            await LoadPermissionData();
            await LoadRolePermissionData();
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

        private void ToggleSwitch(ChangeEventArgs e, int permissionId)
        {
            Console.WriteLine($"RoleId : {id}, PermissionId: {permissionId}, Enabled: {e.Value}");            
        }
    }
}
