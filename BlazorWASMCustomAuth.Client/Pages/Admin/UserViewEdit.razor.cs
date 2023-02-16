using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using BlazorWASMCustomAuth.Client.Services;
using System.Security.Claims;
using MudBlazor;
using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared.Constants;

namespace BlazorWASMCustomAuth.Client.Pages.Admin
{
    public partial class UserViewEdit
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
        public UserDto user { get; set; }
        public UserCreateLocalPasswordDto userPassword { get; set; } = new UserCreateLocalPasswordDto();
        public List<UserRoleGridDto> userRoles { get; set; }
        public List<UserPermissionGridDto> userPermissions { get; set; }
        [Parameter] public int id { get; set; }
        private bool _loaded;
        private bool _EditMode;

        private bool _AccessUsersView;
        private bool _AccessUsersEdit;
        private bool _AccessUserPermissionsView;
        private bool _AccessUserPermissionsEdit;
        private bool _AccessUserRolesEdit;
        private bool _AccessUserRolesView;
        private bool _AccessUserPasswordsCreate;

        private List<BreadcrumbItem> _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Users", href: "admin/users"),
            new BreadcrumbItem("User View/Edit", href: null, disabled: true)
        };

        protected override async Task OnInitializedAsync()
        {
            var state = await authenticationState;
            SetPermissions(state);

            if (!_AccessUsersView)
            {
                _navigationManager.NavigateTo("/noaccess");
            }
            else
            {
                await LoadUserData();
                await LoadUserRoles();
                await LoadUserPermissions();
            }
        }

        private void SetPermissions(AuthenticationState state)
        {
            _AccessUsersView = securityService.HasPermission(state.User, Access.Users.View);
            _AccessUsersEdit = securityService.HasPermission(state.User, Access.Users.Edit);
            _AccessUserPermissionsView = securityService.HasPermission(state.User, Access.UserPermissions.View);
            _AccessUserPermissionsEdit = securityService.HasPermission(state.User, Access.UserPermissions.Edit);
            _AccessUserRolesView = securityService.HasPermission(state.User, Access.UserRoles.View);
            _AccessUserRolesEdit = securityService.HasPermission(state.User, Access.UserRoles.Edit);
            _AccessUserPasswordsCreate = securityService.HasPermission(state.User, Access.UserPasswords.Create);
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

        private async Task ChangePassword()
        {
            userPassword.UserId = id;
            var result = await securityService.UserPasswordCreateAsync(userPassword);

            if (result != null)
            {
                Snackbar.Add("Password Changed", Severity.Success);
                userPassword.Password = "";
            }
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
                    await LoadUserPermissions();
                }
            }
        }

        private async Task TogglePermissionEnabledSwitch(ChangeEventArgs e, int permissionId, bool allow)
        {
            UserPermissionChangeDto userPermissionChangeDto = new UserPermissionChangeDto()
            {
                UserId = id,
                PermissionId = permissionId,
                Enabled = (bool)e.Value,
                Allow = allow
            };
            var result = await securityService.UserPermissionChangeAsync(userPermissionChangeDto);
            if (result != null)
            {
                if (!result.HasError)
                {
                    Snackbar.Add("Permissions Changed", Severity.Warning);
                }
            }
        }

        private async Task TogglePermissionAllowSwitch(ChangeEventArgs e, int permissionId)
        {
            UserPermissionChangeDto userPermissionChangeDto = new UserPermissionChangeDto()
            {
                UserId = id,
                PermissionId = permissionId,
                Enabled = true,
                Allow = (bool)e.Value
            };

            var result = await securityService.UserPermissionChangeAsync(userPermissionChangeDto);
            if (result != null)
            {
                if (!result.HasError)
                {
                    Snackbar.Add("Permissions  Changed", Severity.Warning);
                }
            }
        }
    }
}

