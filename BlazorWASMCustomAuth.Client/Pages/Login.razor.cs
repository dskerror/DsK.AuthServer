﻿using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Pages;
public partial class Login
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    private UserLoginDto userLoginModel = new UserLoginDto();
    private bool _LoginButtonDisabled;
    protected override async Task OnInitializedAsync()
    {

    }

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
        {
            Snackbar.Add("Username and/or Password incorrect", Severity.Error);
        }
        _LoginButtonDisabled = false;
    }

    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

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