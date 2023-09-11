using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BlazorWASMCustomAuth.Security.Shared.Constants;

namespace BlazorWASMCustomAuth.Client.Pages
{
    public partial class AuthenticationProviderCreate
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
        private AuthenticationProviderMappingCreateDto model = new AuthenticationProviderMappingCreateDto();
        private bool _AccessAuthenticationProviderCreate;

        protected override async Task OnInitializedAsync()
        {
            var state = await authenticationState;
            SetPermissions(state);

            if (!_AccessAuthenticationProviderCreate)
                _navigationManager.NavigateTo("/noaccess");
        }

        private void SetPermissions(AuthenticationState state)
        {
            _AccessAuthenticationProviderCreate = securityService.HasPermission(state.User, Access.AuthenticationProvider.Create);
        }

        private async Task Create()
        {
            var result = await securityService.AuthenticationProviderCreateAsync(model);

            if (result != null)
                if (result.HasError)
                    Snackbar.Add(result.Message, Severity.Error);
                else
                {
                    Snackbar.Add(result.Message, Severity.Success);
                    _navigationManager.NavigateTo($"/AuthenticationProviderViewEdit/{result.Result.Id}");
                }
            else
                Snackbar.Add("An Unknown Error Has Occured", Severity.Error);

        }
    }
}
