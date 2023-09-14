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

        [Parameter] public int ApplicationId { get; set; }
        [Parameter] public int ApplicationRoleId { get; set; }
        private bool _loadedPermissionData;
        private bool _loadedApplicationRolePermissionData;
        private bool _AccessApplicationRolesView;
        private bool _AccessApplicationRolesEdit;
        private bool _AccessApplicationRolesPermissionsView;
        private bool _AccessApplicationRolesPermissionsEdit;
        private List<BreadcrumbItem> _breadcrumbs;

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

            _breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem("Applications", href: "Applications"),
                new BreadcrumbItem("Applications View/Edit", href: $"ApplicationViewEdit/{ ApplicationId }"),
                new BreadcrumbItem("Application Role View/Edit", href: null, disabled: true)
            };
        }

        private void SetPermissions(AuthenticationState state)
        {
            _AccessApplicationRolesView = securityService.HasPermission(state.User, Access.ApplicationRoles.View);
            _AccessApplicationRolesEdit = securityService.HasPermission(state.User, Access.ApplicationRoles.Edit);
            _AccessApplicationRolesPermissionsView = securityService.HasPermission(state.User, Access.ApplicationRolesPermissions.View);
            _AccessApplicationRolesPermissionsEdit = securityService.HasPermission(state.User, Access.ApplicationRolesPermissions.Edit);
        }

        private async Task LoadPermissionData()
        {
            var result = await securityService.ApplicationRoleGetAsync(ApplicationRoleId);
            if (result != null)
            {
                applicationRole = result.Result;
                _loadedPermissionData = true;
            }
        }

        private async Task LoadApplicationRolePermissionData()
        {
            var result = await securityService.ApplicationRolePermissionsGetAsync(ApplicationId, ApplicationRoleId);
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

            var result = await securityService.ApplicationRolePermissionChangeAsync(ApplicationRoleId, permissionId, (bool)e.Value);
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
