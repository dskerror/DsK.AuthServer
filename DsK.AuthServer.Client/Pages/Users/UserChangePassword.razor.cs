using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using DsK.AuthServer.Security.Shared;

namespace DsK.AuthServer.Client.Pages.Users;
public partial class UserChangePassword
{   
    [Parameter] public int UserId { get; set; }
    public MyProfileChangePasswordDto model { get; set; }
    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected override async Task OnInitializedAsync()
    {
        model = new MyProfileChangePasswordDto();
    }
    private async Task ChangePassword()
    {
        var result = await securityService.MyProfileChangePasswordAsync(model);

        if (result)
            Snackbar.Add("Password changed successfully", Severity.Success);
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