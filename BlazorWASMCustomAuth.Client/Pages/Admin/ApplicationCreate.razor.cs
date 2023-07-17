using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BlazorWASMCustomAuth.Security.Shared.Constants;

namespace BlazorWASMCustomAuth.Client.Pages.Admin
{
    public partial class ApplicationCreate
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
        private ApplicationCreateDto model = new ApplicationCreateDto();
        private bool _AccessApplicationCreate;

        protected override async Task OnInitializedAsync()
        {
            var state = await authenticationState;
            SetPermissions(state);

            if (!_AccessApplicationCreate)
                _navigationManager.NavigateTo("/noaccess");
        }

        private void SetPermissions(AuthenticationState state)
        {
            _AccessApplicationCreate = securityService.HasPermission(state.User, Access.Application.Create);
        }

        private async Task Create()
        {
            var result = await securityService.ApplicationCreateAsync(model);

            if (result != null)
                if (result.HasError)
                    Snackbar.Add(result.Message, Severity.Error);
                else
                {
                    Snackbar.Add(result.Message, Severity.Success);
                    _navigationManager.NavigateTo($"/admin/ApplicationViewEdit/{result.Result.Id}");
                }
            else
                Snackbar.Add("An Unknown Error Has Occured", Severity.Error);

        }
    }
}
