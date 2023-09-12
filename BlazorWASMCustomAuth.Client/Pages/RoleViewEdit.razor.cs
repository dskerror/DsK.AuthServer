using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BlazorWASMCustomAuth.Security.Shared.Constants;

namespace BlazorWASMCustomAuth.Client.Pages
{
    public partial class RoleViewEdit
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
        public ApplicationRoleDto role { get; set; }
        public List<RolePermissionGridDto> rolePermissions { get; set; }
        [Parameter] public int id { get; set; }
        private bool _loadedPermissionData;
        private bool _loadedRolePermissionData;
        private bool _AccessRolesView;
        private bool _AccessRolesEdit;
        private bool _AccessRolesPermissionsView;
        private bool _AccessRolesPermissionsEdit;


        private List<BreadcrumbItem> _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Roles", href: "roles"),
            new BreadcrumbItem("Role View/Edit", href: null, disabled: true)
        };
        protected override async Task OnInitializedAsync()
        {
            var state = await authenticationState;
            SetPermissions(state);

            if (!_AccessRolesView)
            {
                _navigationManager.NavigateTo("/noaccess");
            }
            else
            {
                await LoadPermissionData();
                await LoadRolePermissionData();
            }
        }

        private void SetPermissions(AuthenticationState state)
        {
            _AccessRolesView = securityService.HasPermission(state.User, Access.Roles.View);
            _AccessRolesEdit = securityService.HasPermission(state.User, Access.Roles.Edit);
            _AccessRolesPermissionsView = securityService.HasPermission(state.User, Access.RolesPermissions.View);
            _AccessRolesPermissionsEdit = securityService.HasPermission(state.User, Access.RolesPermissions.Edit);
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
            Snackbar.Add("Edit canceled", Severity.Warning);
            await LoadPermissionData();
            await LoadRolePermissionData();
        }

        private async Task ToggleSwitch(ChangeEventArgs e, int permissionId)
        {
            //Console.WriteLine($"RoleId : {id}, PermissionId: {permissionId}, Enabled: {e.Value}");

            var result = await securityService.RolePermissionChangeAsync(id, permissionId, (bool)e.Value);
            if (result != null)
            {
                if (!result.HasError)
                {
                    Snackbar.Add("Permission Changed", Severity.Warning);
                }
            }
        }
    }
}
