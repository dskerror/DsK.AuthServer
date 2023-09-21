using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Components;
public partial class UserChangePassword
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    public UserCreateLocalPasswordDto userPassword { get; set; } = new UserCreateLocalPasswordDto();
    [Parameter] public int UserId { get; set; }
    private bool _AccessUserPasswordsCreate;

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);
    }
    private void SetPermissions(AuthenticationState state)
    {
        _AccessUserPasswordsCreate = securityService.HasPermission(state.User, Access.UserPasswords.Create);
    }

    private async Task ChangePassword()
    {
        userPassword.UserId = UserId;
        var result = await securityService.UserPasswordCreateAsync(userPassword);

        if (result != null)
        {
            Snackbar.Add("Password Changed", Severity.Success);
            userPassword.Password = "";
        }
        else
            Snackbar.Add("An Unknown Error Has Occured", Severity.Error);
    }
}
