using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BlazorWASMCustomAuth.Security.Shared.Constants;

namespace BlazorWASMCustomAuth.Client.Pages
{
    public partial class RoleCreate
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
        private ApplicationRoleCreateDto model = new ApplicationRoleCreateDto();
        private bool _AccessRolesCreate;

        protected override async Task OnInitializedAsync()
        {
            var state = await authenticationState;
            SetPermissions(state);

            if (!_AccessRolesCreate)
                _navigationManager.NavigateTo("/noaccess");
        }

        private void SetPermissions(AuthenticationState state)
        {
            _AccessRolesCreate = securityService.HasPermission(state.User, Access.Roles.Create);
        }

        private async Task Create()
        {
            var result = await securityService.ApplicationRoleCreateAsync(model);

            if (result != null)
                if (result.HasError)
                    Snackbar.Add(result.Message, Severity.Error);
                else
                {
                    Snackbar.Add(result.Message, Severity.Success);
                    _navigationManager.NavigateTo($"/roleviewedit/{result.Result.Id}");
                }
            else
                Snackbar.Add("An Unknown Error Has Occured", Severity.Error);

        }
    }
}
