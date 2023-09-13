using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BlazorWASMCustomAuth.Security.Shared.Constants;

namespace BlazorWASMCustomAuth.Client.Pages
{
    public partial class ApplicationRoleCreate
    {
        [Parameter] public int ApplicationId { get; set; }
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
        private ApplicationRoleCreateDto model = new ApplicationRoleCreateDto();
        private bool _AccessApplicaitonRolesCreate;
        private List<BreadcrumbItem> _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Applications", href: "Applications"),            
            new BreadcrumbItem("Application Role Create", href: null, disabled: true)
        };

        protected override async Task OnInitializedAsync()
        {
            var state = await authenticationState;
            SetPermissions(state);

            if (!_AccessApplicaitonRolesCreate)
                _navigationManager.NavigateTo("/noaccess");
        }

        private void SetPermissions(AuthenticationState state)
        {
            _AccessApplicaitonRolesCreate = securityService.HasPermission(state.User, Access.Roles.Create);
        }

        private async Task Create()
        {
            model.ApplicationId = ApplicationId;
            var result = await securityService.ApplicationRoleCreateAsync(model);

            if (result != null)
                if (result.HasError)
                    Snackbar.Add(result.Message, Severity.Error);
                else
                {
                    Snackbar.Add(result.Message, Severity.Success);
                    _navigationManager.NavigateTo($"/applicationroleviewedit/{result.Result.Id}");
                }
            else
                Snackbar.Add("An Unknown Error Has Occured", Severity.Error);

        }
    }
}
