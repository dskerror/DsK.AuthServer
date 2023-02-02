using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages;
public partial class Login
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    private UserLoginDto userLoginModel = new UserLoginDto();

    protected override async Task OnInitializedAsync()
    {

    }

    private async Task SubmitAsync()
    {
        bool result = await securityService.LoginAsync(userLoginModel);
        if (result)
        {
            _navigationManager.NavigateTo("/");
        }
    }

    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    void TogglePasswordVisibility()
    {
        if (_passwordVisibility)
        {
            _passwordVisibility = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput = InputType.Password;
        }
        else
        {
            _passwordVisibility = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput = InputType.Text;
        }
    }
}