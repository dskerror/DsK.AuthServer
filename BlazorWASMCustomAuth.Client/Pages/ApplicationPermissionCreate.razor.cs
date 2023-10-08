using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BlazorWASMCustomAuth.Security.Shared;

namespace BlazorWASMCustomAuth.Client.Pages
{
    public partial class ApplicationPermissionCreate
    {
        [Parameter] public int ApplicationId { get; set; }
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
        private ApplicationPermissionCreateDto model = new ApplicationPermissionCreateDto();
        private bool _AccessCreate;
        private List<BreadcrumbItem> _breadcrumbs;

        protected override async Task OnInitializedAsync()
        {
            var state = await authenticationState;
            SetPermissions(state);

            if (!_AccessCreate)
                _navigationManager.NavigateTo("/NoAccess");

            _breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem("Applications", href: "Applications"),
                new BreadcrumbItem("Application View/Edit", href: $"Application/{ ApplicationId }"),
                new BreadcrumbItem("Application Permission Create", href: null, disabled: true)
            };
        }

        private void SetPermissions(AuthenticationState state)
        {
            _AccessCreate = securityService.HasPermission(state.User, Access.ApplicationPermissions.Create);
        }

        private async Task Create()
        {
            model.ApplicationId = ApplicationId;
            var result = await securityService.ApplicationPermissionCreateAsync(model);

            if (result != null)
                if (result.HasError)
                    Snackbar.Add(result.Message, Severity.Error);
                else
                {
                    Snackbar.Add(result.Message, Severity.Success);
                    _navigationManager.NavigateTo($"/Application/Permission/{ApplicationId}/{result.Result.Id}");
                }
            else
                Snackbar.Add("An Unknown Error Has Occured", Severity.Error);

        }
    }
}
