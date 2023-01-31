using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using BlazorWASMCustomAuth.Client.Services;
using System.Security.Claims;
using MudBlazor;
using System.Drawing;
using System.Security.Policy;
using AutoMapper;

namespace BlazorWASMCustomAuth.Client.Pages.Admin
{
    public partial class UserEdit
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }

        public UserDto user { get; set; }
        [Parameter] public int id { get; set; }
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            var result = await securityService.UserGet(id);
            if (result != null)
            {
                user = result.Result;
                _loaded = true;
            }
        }

        private async Task EditUser()
        {
            var result = await securityService.UserEdit(user);

            if (result != null)
                if (result.HasError)
                    Snackbar.Add(result.Message, Severity.Error);
                else
                    Snackbar.Add(result.Message, Severity.Normal);
            else
                Snackbar.Add("An Unknown Error Has Occured", Severity.Error);
        }
    }
}
