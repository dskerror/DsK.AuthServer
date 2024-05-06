
using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace DsK.AuthServer.Client.Pages.ApplicationUsers;
public partial class ApplicationUsersIndex
{
    [Parameter] public int ApplicationId { get; set; }
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    public List<ApplicationUserGridDto> applicationsUsers { get; set; }

    [Parameter] public EventCallback ApplicationUserChanged { get; set; }
    private bool _AccessEdit;
    private bool _AccessView;


    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (_AccessView)
            await Load();
    }
    private void SetPermissions(AuthenticationState state)
    {
        _AccessView = securityService.HasPermission(state.User, Access.ApplicationUsers.View);
        _AccessEdit = securityService.HasPermission(state.User, Access.ApplicationUsers.Edit);
    }

    private async Task Load()
    {
        var result = await securityService.ApplicationUsersGetAsync(ApplicationId);
        if (result != null)
        {
            applicationsUsers = result.Result;
            //_loadedRolePermissionData = true;
        }
    }

    private async Task ToggleApplicationSwitch(ChangeEventArgs e, int userId)
    {
        var result = await securityService.UserApplicationChangeAsync(userId, ApplicationId, (bool)e.Value);
        if (result != null)
        {
            if (!result.HasError)
            {
                Snackbar.Add("Application User Changed", Severity.Warning);
                await ApplicationUserChanged.InvokeAsync();
                //await LoadUserPermissions();
            }
        }
    }


    //private async Task IsEnabledToggle(ApplicationRoleDto context)
    //{
    //    string IsEnabledToggleHeader = "";
    //    string IsEnabledToggleMessage = "";

    //    if (context.IsEnabled)
    //    {
    //        IsEnabledToggleHeader = "Disable";
    //        IsEnabledToggleMessage = "disable";
    //    }
    //    else
    //    {
    //        IsEnabledToggleHeader = "Enable";
    //        IsEnabledToggleMessage = "enable";
    //    }

    //    var parameters = new DialogParameters();
    //    parameters.Add("ContentText", $"Are you sure you want to {IsEnabledToggleMessage} this role?");
    //    parameters.Add("ButtonText", "Yes");
    //    var dialogresult = DialogService.Show<GenericDialog>($"{IsEnabledToggleHeader} Role", parameters);
    //    var dialogResult = await dialogresult.Result;
    //    if (!dialogResult.Canceled && bool.TryParse(dialogResult.Data.ToString(), out bool resultbool))
    //    {
    //        var result = await securityService.ApplicationRoleDisableEnableAsync(context.Id);

    //        if (result != null)
    //            if (result.HasError)
    //                Snackbar.Add(result.Message, Severity.Error);
    //            else
    //            {
    //                Snackbar.Add(result.Message, Severity.Success);
    //                await _table.ReloadServerData();
    //            }

    //        else
    //            Snackbar.Add("An Unknown Error Has Occured", Severity.Error);
    //    }
    //    else
    //    {
    //        Snackbar.Add("Operation Canceled", Severity.Warning);
    //    }
    //}
}
