using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using BlazorWASMCustomAuth.Client.Services;
using System.Security.Claims;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages.Admin
{
    public partial class UserViewEdit
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }

        public UserDto user { get; set; }
        [Parameter] public int id { get; set; }
        private bool _loaded;
        private bool _EditMode;
        private List<BreadcrumbItem> _breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Users", href: "admin/users"),
            new BreadcrumbItem("User View/Edit", href: null, disabled: true)
        };

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var result = await securityService.UserGetAsync(id);
            if (result != null)
            {
                user = result.Result;
                _loaded = true;
            }
        }

        private async Task EditUser()
        {
            var result = await securityService.UserEditAsync(user);
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
