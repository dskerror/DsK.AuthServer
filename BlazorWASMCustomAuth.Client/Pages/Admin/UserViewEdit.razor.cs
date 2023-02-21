using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BlazorWASMCustomAuth.Security.Shared.Constants;
using BlazorWASMCustomAuth.Security.Infrastructure;

namespace BlazorWASMCustomAuth.Client.Pages.Admin
{
    public partial class UserViewEdit
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
        public UserDto user { get; set; }
        public UserCreateLocalPasswordDto userPassword { get; set; } = new UserCreateLocalPasswordDto();
        public List<UserRoleGridDto> userRoles { get; set; }
        public List<UserPermissionGridDto> userPermissions { get; set; }

        public List<UserAuthenticationProvidersGridDto> userAuthenticationProviders { get; set; }
        [Parameter] public int id { get; set; }
        private bool _loaded;

        private bool _AccessUsersView;
        private bool _AccessUsersEdit;
        private bool _AccessUserPermissionsView;
        private bool _AccessUserPermissionsEdit;
        private bool _AccessUserRolesEdit;
        private bool _AccessUserRolesView;
        private bool _AccessUserPasswordsCreate;
        private bool _AccessUserAuthenticationProvidersView;
        private bool _AccessUserAuthenticationProvidersCreate;
        private bool _AccessUserAuthenticationProvidersEdit;
        private bool _AccessUserAuthenticationProvidersDelete;

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
                if (_AccessUserRolesView)
                    await LoadUserRoles();
                if (_AccessUserPermissionsView)
                    await LoadUserPermissions();
                if (_AccessUserAuthenticationProvidersView)
                    await LoadUserAuthenticationProviders();
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
            _AccessUserAuthenticationProvidersView = securityService.HasPermission(state.User, Access.UserAuthenticationProviders.View);
            _AccessUserAuthenticationProvidersCreate = securityService.HasPermission(state.User, Access.UserAuthenticationProviders.Create);
            _AccessUserAuthenticationProvidersEdit = securityService.HasPermission(state.User, Access.UserAuthenticationProviders.Edit);
            _AccessUserAuthenticationProvidersDelete = securityService.HasPermission(state.User, Access.UserAuthenticationProviders.Delete);
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

        private async Task LoadUserAuthenticationProviders()
        {
            var result = await securityService.UserAuthenticationProvidersGetAsync(id);
            if (result != null)
            {
                userAuthenticationProviders = result.Result;
            }
        }

        private async Task EditUser()
        {
            var result = await securityService.UserEditAsync(user);

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
            Snackbar.Add("Edit canceled", Severity.Warning);
            await LoadUserData();
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
        private async Task SaveUserAuthenticatonProvider(UserAuthenticationProvidersGridDto record)
        {
            if (_AccessUserAuthenticationProvidersDelete && string.IsNullOrEmpty(record.Username) && record.Id != 0)
            {
                var result = await securityService.UserAuthenticationProviderDeleteAsync(record.Id);
                if (result != null)
                {
                    if (!result.HasError)
                        Snackbar.Add("User Authentication Provider Deleted", Severity.Success);
                }
            }
            else if (!_AccessUserAuthenticationProvidersDelete && string.IsNullOrEmpty(record.Username) && record.Id != 0)
            {
                Snackbar.Add("You don't have permission to delete the User Authentication Provider", Severity.Success);
            }
            else if (_AccessUserAuthenticationProvidersEdit && !string.IsNullOrEmpty(record.Username) && record.Id != 0)
            {
                UserAuthenticationProviderUpdateDto userAuthenticationProviderUpdateDto = new UserAuthenticationProviderUpdateDto()
                {
                    Id = record.Id,
                    Username = record.Username
                };
                var result = await securityService.UserAuthenticationProviderEditAsync(userAuthenticationProviderUpdateDto);
                if (result != null)
                {
                    if (!result.HasError)
                        Snackbar.Add("User Authentication Provider Edited", Severity.Success);
                    else
                        Snackbar.Add(result.Message, Severity.Error);
                }
            }
            else if (!_AccessUserAuthenticationProvidersEdit && !string.IsNullOrEmpty(record.Username) && record.Id != 0)
            {
                Snackbar.Add("You don't have permission to edit the User Authentication Provider", Severity.Success);
            }
            else if (_AccessUserAuthenticationProvidersCreate && !string.IsNullOrEmpty(record.Username) && record.Id == 0)
            {
                UserAuthenticationProviderCreateDto userAuthenticationProviderCreateDto = new UserAuthenticationProviderCreateDto()
                {
                    AuthenticationProviderId = record.AuthenticationProviderId,
                    UserId = id,
                    Username = record.Username
                };
                var result = await securityService.UserAuthenticationProviderCreateAsync(userAuthenticationProviderCreateDto);
                if (result != null)
                {
                    if (!result.HasError)
                        Snackbar.Add("User Authentication Provider Created", Severity.Success);
                    else
                        Snackbar.Add(result.Message, Severity.Error);
                }
            }
            else if (!_AccessUserAuthenticationProvidersCreate && !string.IsNullOrEmpty(record.Username) && record.Id == 0)
            {
                Snackbar.Add("You don't have permission to create the User Authentication Provider", Severity.Success);
            }

            await LoadUserAuthenticationProviders();
        }
    }
}