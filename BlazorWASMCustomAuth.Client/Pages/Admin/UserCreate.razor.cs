using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using BlazorWASMCustomAuth.Client.Services;
using System.Security.Claims;
using BlazorWASMCustomAuth.Client.Components;

namespace BlazorWASMCustomAuth.Client.Pages.Admin
{
    public partial class UserCreate
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
  
        private UserCreateDto userCreateModel = new UserCreateDto();
        private Notification _notification;

        //private APIResult apiresult;



        //protected override async Task OnInitializedAsync()
        //{

        //}

        private async Task CreateUser()
        {
            var result = await securityService.UserCreate(userCreateModel);

            if (result != null)
            {
                if (result.HasError)
                {
                    _notification.Show("User Create",result.Message);
                }
                else
                {
                    _notification.Show("User Create", result.Message);
                }

            }
            else
            {
                _notification.Show("User Create", "An Unknown Error Has Occured");
            }
        }
    }
}
