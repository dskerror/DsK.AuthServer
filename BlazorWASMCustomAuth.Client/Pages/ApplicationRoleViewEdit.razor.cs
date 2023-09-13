using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BlazorWASMCustomAuth.Security.Shared.Constants;

namespace BlazorWASMCustomAuth.Client.Pages
{
    public partial class ApplicationRoleViewEdit
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
        public ApplicationRoleDto applicationRole { get; set; }
        public List<ApplicationRolePermissionGridDto> applicationRolePermissions { get; set; }
        [Parameter] public int id { get; set; }
        private bool _loadedPermissionData;
        private bool _loadedApplicationRolePermissionData;
        private bool _AccessApplicationRolesView;
        private bool _AccessApplicationRolesEdit;
        private bool _AccessApplicationRolesPermissionsView;
        private bool _AccessApplicationRolesPermissionsEdit;


        private List<BreadcrumbItem> _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Application Roles", href: "applicationroles"),
            new BreadcrumbItem("Application Role View/Edit", href: null, disabled: true)
        };
        protected override async Task OnInitializedAsync()
        {
            var state = await authenticationState;
            SetPermissions(state);

            if (!_AccessApplicationRolesView)
            {
                _navigationManager.NavigateTo("/noaccess");
            }
            else
            {
                await LoadPermissionData();
                await LoadApplicationRolePermissionData();
            }
        }

        private void SetPermissions(AuthenticationState state)
        {
            _AccessApplicationRolesView = securityService.HasPermission(state.User, Access.Roles.View);
            _AccessApplicationRolesEdit = securityService.HasPermission(state.User, Access.Roles.Edit);
            _AccessApplicationRolesPermissionsView = securityService.HasPermission(state.User, Access.RolesPermissions.View);
            _AccessApplicationRolesPermissionsEdit = securityService.HasPermission(state.User, Access.RolesPermissions.Edit);
        }

        private async Task LoadPermissionData()
        {
            var result = await securityService.ApplicationRoleGetAsync(id);
            if (result != null)
            {
                applicationRole = result.Result;
                _loadedPermissionData = true;
            }
        }

        private async Task LoadApplicationRolePermissionData()
        {
            var result = await securityService.ApplicationRolePermissionsGetAsync(id);
            if (result != null)
            {
                applicationRolePermissions = result.Result;
                _loadedApplicationRolePermissionData = true;
            }
        }

        private async Task EditApplicationRole()
        {
            var result = await securityService.ApplicationRoleEditAsync(applicationRole);
            
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
            await LoadApplicationRolePermissionData();
        }

        private async Task ToggleSwitch(ChangeEventArgs e, int permissionId)
        {
            //Console.WriteLine($"RoleId : {id}, PermissionId: {permissionId}, Enabled: {e.Value}");

            var result = await securityService.ApplicationRolePermissionChangeAsync(id, permissionId, (bool)e.Value);
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
