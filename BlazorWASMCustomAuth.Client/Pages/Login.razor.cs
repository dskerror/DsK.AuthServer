using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages;
public partial class Login
{
    [Parameter] public string ApplicationAuthenticationProviderGUID { get; set; }
    private UserLoginDto userLoginModel = new UserLoginDto() { ApplicationAuthenticationProviderGUID = Guid.Parse("BB54D562-58A7-4267-8F9A-0B1A60A12C02") };
    private bool _LoginButtonDisabled;
    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    private async Task SubmitAsync()
    {
        _LoginButtonDisabled = true;
        
        bool result = await securityService.LoginAsync(userLoginModel);

        if (result)
        {
            if (ApplicationAuthenticationProviderGUID != null)
            {
                _navigationManager.NavigateTo("/");
                Snackbar.Add("Login Successful", Severity.Success);
            } else
            {
                _navigationManager.NavigateTo("https://localhost:7298");
            }
        }
        else
            Snackbar.Add("Username and/or Password incorrect", Severity.Error);

        _LoginButtonDisabled = false;
    }
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