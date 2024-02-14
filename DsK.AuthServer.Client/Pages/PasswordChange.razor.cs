using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using DsK.AuthServer.Security.Shared;

namespace DsK.AuthServer.Client.Pages;
public partial class PasswordChange
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    [Parameter] public string PasswordChangeGuid { get; set; }
    private PasswordChangeDto model { get; set; }
    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected override async Task OnInitializedAsync()
    {
        model = new PasswordChangeDto() { PasswordChangeGuid = Guid.Parse(PasswordChangeGuid) };
    }

    private async Task ChangePassword()
    {
        //todo : PasswordChangePage : compare both passwords
        //todo : PasswordChangePage : password complexity
        //todo : PasswordChangePage : password complexity

        var result = await securityService.PasswordChangeAsync(model);

        if (result)
        {
            Snackbar.Add("Password changed successfully", Severity.Success);
            _navigationManager.NavigateTo("login");
        }
        else
            Snackbar.Add("Couldn't change password", Severity.Error);
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