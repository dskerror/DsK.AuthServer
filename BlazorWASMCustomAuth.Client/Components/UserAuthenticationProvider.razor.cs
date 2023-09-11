using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using BlazorWASMCustomAuth.Security.Shared.Constants;
using static BlazorWASMCustomAuth.Security.Shared.Constants.Access;
using BlazorWASMCustomAuth.Security.Shared;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Components
{
    public partial class UserAuthenticationProvider
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
        public List<UserAuthenticationProviderMappingsGridDto> userAuthenticationProviders { get; set; }

        [Parameter] public int UserId { get; set; }

        private bool _AccessUserAuthenticationProvidersView;
        private bool _AccessUserAuthenticationProvidersCreate;
        private bool _AccessUserAuthenticationProvidersEdit;
        private bool _AccessUserAuthenticationProvidersDelete;


        protected override async Task OnInitializedAsync()
        {
            var state = await authenticationState;
            SetPermissions(state);

            if (_AccessUserAuthenticationProvidersView)
            {
                    await LoadUserAuthenticationProviders();
            }
        }
        private void SetPermissions(AuthenticationState state)
        {
            _AccessUserAuthenticationProvidersView = securityService.HasPermission(state.User, Access.UserAuthenticationProviders.View);
            _AccessUserAuthenticationProvidersCreate = securityService.HasPermission(state.User, Access.UserAuthenticationProviders.Create);
            _AccessUserAuthenticationProvidersEdit = securityService.HasPermission(state.User, Access.UserAuthenticationProviders.Edit);
            _AccessUserAuthenticationProvidersDelete = securityService.HasPermission(state.User, Access.UserAuthenticationProviders.Delete);
        }

        private async Task LoadUserAuthenticationProviders()
        {
            var result = await securityService.UserAuthenticationProvidersGetAsync(UserId);
            if (result != null)
            {
                userAuthenticationProviders = result.Result;
            }
        }

        private async Task SaveUserAuthenticatonProvider(UserAuthenticationProviderMappingsGridDto record)
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
                    UserId = UserId,
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
