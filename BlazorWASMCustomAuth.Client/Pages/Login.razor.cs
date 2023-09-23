using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages;
public partial class Login
{
    [Parameter] public string ApplicationAuthenticationProviderGUID { get; set; }
    private LoginRequestDto userLoginModel = new LoginRequestDto();
    private bool _LoginButtonDisabled;
    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    private async Task SubmitAsync()
    {
        _LoginButtonDisabled = true;

        if (ApplicationAuthenticationProviderGUID != null)
            userLoginModel.ApplicationAuthenticationProviderGUID = Guid.Parse(ApplicationAuthenticationProviderGUID);

        var result = await securityService.LoginAsync(userLoginModel);

        if (!result.IsNullOrEmpty())
        {
            _navigationManager.NavigateTo(result);
            Snackbar.Add("Authenticating...", Severity.Success);
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