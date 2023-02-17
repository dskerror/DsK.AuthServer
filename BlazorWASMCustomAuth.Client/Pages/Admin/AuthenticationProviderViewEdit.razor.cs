using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BlazorWASMCustomAuth.Security.Shared.Constants;

namespace BlazorWASMCustomAuth.Client.Pages.Admin
{
    public partial class AuthenticationProviderViewEdit
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }

        public AuthenticationProviderDto model { get; set; }
        [Parameter] public int id { get; set; }
        private bool _loaded;        
        private bool _AccessAuthenticationProviderView;
        private bool _AccessAuthenticationProviderEdit;
        private List<BreadcrumbItem> _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("AuthenticationProviders", href: "admin/AuthenticationProviders"),
            new BreadcrumbItem("Authentication Provider View/Edit", href: null, disabled: true)
        };

        protected override async Task OnInitializedAsync()
        {
            var state = await authenticationState;
            SetPermissions(state);

            if (!_AccessAuthenticationProviderView)            
                _navigationManager.NavigateTo("/noaccess");            
            else            
                await LoadData();
        }

        private void SetPermissions(AuthenticationState state)
        {
            _AccessAuthenticationProviderView = securityService.HasPermission(state.User, Access.AuthenticationProvider.View);
            _AccessAuthenticationProviderEdit = securityService.HasPermission(state.User, Access.AuthenticationProvider.Edit);
        }

        private async Task LoadData()
        {
            var result = await securityService.AuthenticationProviderGetAsync(id);
            if (result != null)
            {
                model = result.Result;
                _loaded = true;
            }
        }

        private async Task Edit()
        {
            var result = await securityService.AuthenticationProviderEditAsync(model);
          
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
            await LoadData();
        }
    }
}
