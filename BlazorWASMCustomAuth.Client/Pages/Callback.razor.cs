using Microsoft.AspNetCore.Components;
using MudBlazor;
namespace BlazorWASMCustomAuth.Client.Pages;
public partial class Callback
{
    [Parameter] public string LoginToken { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (LoginToken != null)
        {
            
            var result = await securityService.ValidateLoginTokenAsync(LoginToken);

            if (result)
            {
                _navigationManager.NavigateTo("/");
                Snackbar.Add("Authentication successful", Severity.Success);
            }
            else
            {
                _navigationManager.NavigateTo("/NoAccess");
                Snackbar.Add("Username and/or Password incorrect", Severity.Error);
            }
        }        
    }
}