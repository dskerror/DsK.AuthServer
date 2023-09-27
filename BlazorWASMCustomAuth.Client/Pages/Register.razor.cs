using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages;
public partial class Register
{
    [Parameter] public string ApplicationAuthenticationProviderGUID { get; set; }
    private RegisterRequestDto userRegisterModel = new RegisterRequestDto();
    private bool _LoginButtonDisabled;
    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    private async Task SubmitAsync()
    {
        _LoginButtonDisabled = true;

        if (ApplicationAuthenticationProviderGUID != null)
            userRegisterModel.ApplicationAuthenticationProviderGUID = Guid.Parse(ApplicationAuthenticationProviderGUID);

        var result = await securityService.RegisterAsync(userRegisterModel);

        if (result)
        {
            _navigationManager.NavigateTo($"/login/{ApplicationAuthenticationProviderGUID}");
            Snackbar.Add("Register successful. Please login.", Severity.Success);
        }
        else
            Snackbar.Add("Register failed.", Severity.Error);

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