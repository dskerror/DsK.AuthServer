using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using MudBlazor;

namespace DsK.AuthServer.Client.Pages;
public partial class Register
{
    [Parameter] public string ApplicationAuthenticationProviderGUID { get; set; }
    private RegisterRequestDto userRegisterModel = new RegisterRequestDto();
    private ApplicationAuthenticationProviderValidateDto appAuthProvValidModel = new ApplicationAuthenticationProviderValidateDto();
    private bool _LoginButtonDisabled;
    private bool _passwordVisibility;
    private string _EmailLabel = "Email";
    private bool _GuidIsValid;
    private bool _IsLoaded = false;
    private bool _RegistrationEnabled = true;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;


    protected override async Task OnInitializedAsync()
    {
        if (ApplicationAuthenticationProviderGUID != null)
        {
            Guid guid = Guid.Empty;
            Guid.TryParse(ApplicationAuthenticationProviderGUID, out guid);
            userRegisterModel.ApplicationAuthenticationProviderGUID = guid;
            appAuthProvValidModel = await securityService.ValidateApplicationAuthenticationProviderGuid(guid.ToString());
            if (appAuthProvValidModel != null && appAuthProvValidModel.Id != 0)
            {
                _GuidIsValid = true;
                _RegistrationEnabled = appAuthProvValidModel.RegistrationEnabled;
                if (appAuthProvValidModel.AuthenticationProviderType == "Active Directory")
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
        //if (userRegisterModel.ADUsername == null) { userRegisterModel.ADUsername = userRegisterModel.Email};
        userRegisterModel.ADUsername ??= userRegisterModel.Email;

        var result = await securityService.RegisterAsync(userRegisterModel);


        if (result.HasError)        
            Snackbar.Add("Register failed.", Severity.Error);         
        else
        {
            _navigationManager.NavigateTo($"login/{ApplicationAuthenticationProviderGUID}");
            Snackbar.Add(result.Message, Severity.Success);
        }   

        _LoginButtonDisabled = false;
    }

    private void GoBackToLogin()
    {
        if (ApplicationAuthenticationProviderGUID != null)
            _navigationManager.NavigateTo($"Login/{ApplicationAuthenticationProviderGUID}");
        else
            _navigationManager.NavigateTo("Login");
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