using BlazorWASMCustomAuth.Client.Services;
using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorWASMCustomAuth.Client.Pages.Admin
{
    public partial class Users
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }        
        
        private List<Claim> userClaims;
        private APIResultNew<List<UserDto>> response;
        private List<UserDto> users;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            var auth = authenticationState.Result;
            if (auth.User.Identity.IsAuthenticated)
            {
                userClaims = auth.User.Claims.ToList();
            }
            response = await securityService.UsersGet();

            if(response != null)
            {
                users = response.Result;
                _loaded = true;
            }
            
        }

        private void EditUser(int id)
        {

        }

        private void ViewUser(int id)
        {

        }

        private void DeleteUser(int id)
        {

        }

        private void CreateUser()
        {
            _navigationManager.NavigateTo("/admin/usercreate");

            //<NavLink href="/admin/usercreate">Create User</NavLink>
        }
    }
}
