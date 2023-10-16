using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using DsK.AuthServer.Security.Shared;

namespace DsK.AuthServer.Client.Components;
public partial class MyProfileChangePassword
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    public MyProfileChangePasswordDto model { get; set; }
    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected override async Task OnInitializedAsync()
    {
        model = new MyProfileChangePasswordDto();
        int userId = await GetUsedIdFromAuthenticationState();
        model.UserId = userId;
    }

    private async Task<int> GetUsedIdFromAuthenticationState()
    {
        var state = await authenticationState;
        var userIdString = state.User.Claims.Where(_ => _.Type == "UserId").Select(_ => _.Value).FirstOrDefault();
        int userId = int.Parse(userIdString);
        return userId;
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