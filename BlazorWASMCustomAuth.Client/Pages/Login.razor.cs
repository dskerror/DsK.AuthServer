using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages;
public partial class Login
{
    [Parameter] public string ApplicationAuthenticationProviderGUID { get; set; }
    private LoginRequestDto userLoginModel = new LoginRequestDto();
    private ApplicationAuthenticationProviderValidateDto appAuthProvValidModel = new ApplicationAuthenticationProviderValidateDto();
    private bool _LoginButtonDisabled;
    private bool _passwordVisibility;
    private bool _GuidIsValid;
    private bool _IsLoaded = false;
    private bool _RegistrationEnabled = true;
    private string _EmailLabel = "Email";
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected override async Task OnInitializedAsync()
    {
        if (ApplicationAuthenticationProviderGUID != null)
        {
            Guid guid = Guid.Empty;
            Guid.TryParse(ApplicationAuthenticationProviderGUID, out guid);
            userLoginModel.ApplicationAuthenticationProviderGUID = guid;
            appAuthProvValidModel = await securityService.ValidateApplicationAuthenticationProviderGuid(guid.ToString());
            if (appAuthProvValidModel != null && appAuthProvValidModel.Id != 0) { 
                _GuidIsValid = true;
                _RegistrationEnabled = appAuthProvValidModel.RegistrationEnabled;
                if(appAuthProvValidModel.AuthenticationProviderType == "Active Directory")
                {
                    _EmailLabel = "Username";
                }
            }
        }
        else
        {
            _GuidIsValid = true;
        }
        _IsLoaded = true;
    }

    private async Task SubmitAsync()
    {
        _LoginButtonDisabled = true;

        //if (ApplicationAuthenticationProviderGUID != null)
        //    userLoginModel.ApplicationAuthenticationProviderGUID = Guid.Parse(ApplicationAuthenticationProviderGUID);

        var result = await securityService.LoginAsync(userLoginModel);

        if (result != null)
        {
            await securityService.ValidateLoginTokenAsync(result.LoginToken.ToString());
            _navigationManager.NavigateTo(result.CallbackURL);
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

    private void ChangePassword()
    {
        _navigationManager.NavigateTo("/PasswordChangeRequest");
    }

    private void Register()
    {
        _navigationManager.NavigateTo("/Register");
    }
}