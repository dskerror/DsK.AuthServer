using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using BlazorWASMCustomAuth.Client.Services;
using System.Security.Claims;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages.Admin
{
    public partial class UserViewEdit
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }

        public UserDto user { get; set; }
        public List<UserRoleGridDto> userRoles { get; set; }
        public List<UserPermissionGridDto> userPermissions { get; set; }
        [Parameter] public int id { get; set; }
        private bool _loaded;
        private bool _EditMode;
        private List<BreadcrumbItem> _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Users", href: "admin/users"),
            new BreadcrumbItem("User View/Edit", href: null, disabled: true)
        };

        protected override async Task OnInitializedAsync()
        {
            await LoadUserData();
            await LoadUserRoles();
            await LoadUserPermissions();
        }

        private async Task LoadUserData()
        {
            var result = await securityService.UserGetAsync(id);
            if (result != null)
            {
                user = result.Result;
                _loaded = true;
            }
        }

        private async Task LoadUserRoles()
        {
            var result = await securityService.UserRolesGetAsync(id);
            if (result != null)
            {
                userRoles = result.Result;
                //_loadedRolePermissionData = true;
            }
        }

        private async Task LoadUserPermissions()
        {
            var result = await securityService.UserPermissionsGetAsync(id);
            if (result != null)
            {
                userPermissions = result.Result;
                //_loadedRolePermissionData = true;
            }
        }

        private async Task EditUser()
        {
            var result = await securityService.UserEditAsync(user);
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
            await LoadUserData();
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

        private async Task ToggleRoleSwitch(ChangeEventArgs e, int roleId)
        {
            //Console.WriteLine($"RoleId : {id}, PermissionId: {permissionId}, Enabled: {e.Value}");

            var result = await securityService.UserRoleChangeAsync(id, roleId, (bool)e.Value);
            if (result != null)
            {
                if (!result.HasError)
                {
                    Snackbar.Add("Role Changed", Severity.Warning);
                }
            }
        }

        private async Task TogglePermissionSwitch(ChangeEventArgs e, int roleId)
        {
            //Console.WriteLine($"RoleId : {id}, PermissionId: {permissionId}, Enabled: {e.Value}");

            var result = await securityService.UserRoleChangeAsync(id, roleId, (bool)e.Value);
            if (result != null)
            {
                if (!result.HasError)
                {
                    Snackbar.Add("Role Changed", Severity.Warning);
                }
            }
        }
    }
}
