using BlazorWASMCustomAuth.Security.Shared;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages;
public partial class Login
{
    private UserLoginDto userLoginModel = new UserLoginDto() { AuthenticationProviderId = 3};
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
            _navigationManager.NavigateTo("/");
            Snackbar.Add("Login Successful", Severity.Success);
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