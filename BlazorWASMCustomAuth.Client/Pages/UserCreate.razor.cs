using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages;
public partial class UserCreate
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    private UserCreateDto model = new UserCreateDto();
    private bool _AccessCreate;

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessCreate)
            _navigationManager.NavigateTo("/noaccess");
    }

    private void SetPermissions(AuthenticationState state)
    {   
        _AccessCreate = securityService.HasPermission(state.User, Access.Users.Create);
    }

    private async Task Create()
    {
        var result = await securityService.UserCreateAsync(model);

        if (result != null)
            if (result.HasError)
                Snackbar.Add(result.Message, Severity.Error);
            else
            {
                Snackbar.Add(result.Message, Severity.Success);
                _navigationManager.NavigateTo($"/userviewedit/{result.Result.Id}");
            }
        else
            Snackbar.Add("An Unknown Error Has Occured", Severity.Error);

    }
}
