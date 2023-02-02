using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using BlazorWASMCustomAuth.Client.Services;
using System.Security.Claims;
using MudBlazor;
using System.Drawing;
using System.Security.Policy;

namespace BlazorWASMCustomAuth.Client.Pages.Admin
{
    public partial class RoleCreate
    {
        [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }

        private RoleCreateDto roleCreateModel = new RoleCreateDto();
        //private APIResult apiresult;



        //protected override async Task OnInitializedAsync()
        //{

        //}

        private async Task CreateUser()
        {
            var result = await securityService.RoleCreate(roleCreateModel);

            if (result != null)
                if (result.HasError)
                    Snackbar.Add(result.Message, Severity.Error);
                else
                {
                    Snackbar.Add(result.Message, Severity.Success);
                    _navigationManager.NavigateTo($"/admin/roleviewedit/{result.Result.Id}");
                }
            else
                Snackbar.Add("An Unknown Error Has Occured", Severity.Error);

        }
    }
}
