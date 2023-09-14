using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using BlazorWASMCustomAuth.Security.Shared.Constants;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Components
{   
    public partial class ApplicationPermissions
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
        public List<ApplicationPermissionGridDto> applicationPermissions { get; set; }
        [Parameter] public int ApplicationId { get; set; }

        private bool _AccessApplicationPermissionsView;
        private bool _AccessApplicationPermissionsEdit;
        private bool _AccessApplicationPermissionsCreate;

        protected override async Task OnInitializedAsync()
        {
            var state = await authenticationState;
            SetPermissions(state);
            
            if (_AccessApplicationPermissionsView)
                await LoadUserPermissions();
        }
        private void SetPermissions(AuthenticationState state)
        {
            _AccessApplicationPermissionsView = securityService.HasPermission(state.User, Access.ApplicationPermissions.View);
            _AccessApplicationPermissionsEdit = securityService.HasPermission(state.User, Access.ApplicationPermissions.Edit);
            _AccessApplicationPermissionsCreate = securityService.HasPermission(state.User, Access.ApplicationPermissions.Create);
        }
        public async Task LoadUserPermissions()
        {
            var result = await securityService.ApplicationPermissionsGetAsync(ApplicationId);
            if (result != null)
            {
                applicationPermissions = result.Result;
            }
            StateHasChanged();
        }

        private void CreateApplicationPermissions()
        {
            _navigationManager.NavigateTo($"/ApplicationPermissionsCreate/{ApplicationId}");
        }
    }
}
