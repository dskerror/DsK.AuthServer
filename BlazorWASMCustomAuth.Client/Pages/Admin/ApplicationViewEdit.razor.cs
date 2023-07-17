using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BlazorWASMCustomAuth.Security.Shared.Constants;

namespace BlazorWASMCustomAuth.Client.Pages.Admin
{
    public partial class ApplicationViewEdit
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }

        public ApplicationDto model { get; set; }
        [Parameter] public int id { get; set; }
        private bool _loaded;        
        private bool _AccessApplicationView;
        private bool _AccessApplicationEdit;
        private List<BreadcrumbItem> _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Applications", href: "admin/Applications"),
            new BreadcrumbItem("Application View/Edit", href: null, disabled: true)
        };

        protected override async Task OnInitializedAsync()
        {
            var state = await authenticationState;
            SetPermissions(state);

            if (!_AccessApplicationView)            
                _navigationManager.NavigateTo("/noaccess");            
            else            
                await LoadData();
        }

        private void SetPermissions(AuthenticationState state)
        {
            _AccessApplicationView = securityService.HasPermission(state.User, Access.Application.View);
            _AccessApplicationEdit = securityService.HasPermission(state.User, Access.Application.Edit);
        }

        private async Task LoadData()
        {
            var result = await securityService.ApplicationGetAsync(id);
            if (result != null)
            {
                model = result.Result;
                _loaded = true;
            }
        }

        private async Task Edit()
        {
            var result = await securityService.ApplicationEditAsync(model);
          
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
