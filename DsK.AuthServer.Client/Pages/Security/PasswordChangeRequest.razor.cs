using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DsK.AuthServer.Client.Pages.Security;
public partial class PasswordChangeRequest
{
    [Parameter] public string ApplicationAuthenticationProviderGUID { get; set; }
    private PasswordChangeRequestDto model = new PasswordChangeRequestDto();

    private async Task SubmitAsync()
    {
        if (ApplicationAuthenticationProviderGUID != null)
            model.ApplicationAuthenticationProviderGUID = Guid.Parse(ApplicationAuthenticationProviderGUID);

        var result = await securityService.PasswordChangeRequestAsync(model);

        if (result)
        {
            _navigationManager.NavigateTo("login");
            Snackbar.Add("Password change request sent to email.", Severity.Success);
        }
        else
            Snackbar.Add("Error", Severity.Error);
    }
}