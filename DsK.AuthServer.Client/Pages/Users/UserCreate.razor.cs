using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DsK.AuthServer.Client.Pages.Users;
public partial class UserCreate
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    private UserCreateDto model = new UserCreateDto();
    private bool _AccessCreate;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
    private bool _passwordVisibility;
    private List<BreadcrumbItem> _breadcrumbs = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Users", href: "users"),
        new BreadcrumbItem("User Create", href: null, disabled: true)
    };

    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (!_AccessCreate)
            _navigationManager.NavigateTo("NoAccess");
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
                _navigationManager.NavigateTo($"User/{result.Result.Id}");
            }
        else
            Snackbar.Add("An Unknown Error Has Occured", Severity.Error);

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
