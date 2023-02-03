using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages.Admin
{
    public partial class AuthenticationProviderEdit
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }

        public AuthenticationProviderDto model { get; set; }
        [Parameter] public int id { get; set; }
        private bool _loaded;
        private bool _EditMode;
        private List<BreadcrumbItem> _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("AuthenticationProviders", href: "admin/AuthenticationProviders"),
            new BreadcrumbItem("Authentication Provider View/Edit", href: null, disabled: true)
        };

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
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
            await LoadData();
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
    }
}
