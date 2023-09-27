using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using BlazorWASMCustomAuth.Security.Shared;
using MudBlazor;

namespace BlazorWASMCustomAuth.Client.Components;
public partial class ApplicationAuthenticationProviderUserMappings
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    public List<ApplicationAuthenticationProviderUserMappingsGridDto> applicationAuthenticationProviderUserMappings { get; set; }

    [Parameter] public int UserId { get; set; }

    private bool _AccessView;
    private bool _AccessCreate;
    private bool _AccessEdit;
    private bool _AccessDelete;


    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (_AccessView)
        {
                await LoadApplicationAuthenticationProviderUserMappings();
        }
    }
    private void SetPermissions(AuthenticationState state)
    {
        _AccessView = securityService.HasPermission(state.User, Access.ApplicationAuthenticationProviderUserMappings.View);
        _AccessCreate = securityService.HasPermission(state.User, Access.ApplicationAuthenticationProviderUserMappings.Create);
        _AccessEdit = securityService.HasPermission(state.User, Access.ApplicationAuthenticationProviderUserMappings.Edit);
        _AccessDelete = securityService.HasPermission(state.User, Access.ApplicationAuthenticationProviderUserMappings.Delete);
    }

    private async Task LoadApplicationAuthenticationProviderUserMappings()
    {   
        var result = await securityService.ApplicationAuthenticationProviderUserMappingsGetAsync(UserId);
        if (result != null)
        {
            //applicationAuthenticationProviderUserMappings = result.Result;
        }
    }

    private async Task SaveUserAuthenticatonProvider(ApplicationAuthenticationProviderUserMappingsGridDto record)
    {
        if (_AccessDelete && string.IsNullOrEmpty(record.Username) && record.Id != 0)
        {
            var result = await securityService.ApplicationAuthenticationProviderUserMappingDeleteAsync(record.Id);
            if (result != null)
            {
                if (!result.HasError)
                    Snackbar.Add("User Mapping Deleted", Severity.Success);
            }
        }
        else if (!_AccessDelete && string.IsNullOrEmpty(record.Username) && record.Id != 0)
        {
            Snackbar.Add("You don't have permission to delete the User Authentication Provider", Severity.Success);
        }
        else if (_AccessEdit && !string.IsNullOrEmpty(record.Username) && record.Id != 0)
        {
            ApplicationAuthenticationProviderUserMappingUpdateDto applicationAuthenticationProviderUserMappingUpdateDto = new ApplicationAuthenticationProviderUserMappingUpdateDto()
            {
                Id = record.Id,
                Username = record.Username
            };
            var result = await securityService.ApplicationAuthenticationProviderUserMappingEditAsync(applicationAuthenticationProviderUserMappingUpdateDto);
            if (result != null)
            {
                if (!result.HasError)
                    Snackbar.Add("User Mapping Edited", Severity.Success);
                else
                    Snackbar.Add(result.Message, Severity.Error);
            }
        }
        else if (!_AccessEdit && !string.IsNullOrEmpty(record.Username) && record.Id != 0)
        {
            Snackbar.Add("You don't have permission to edit the User Mapping", Severity.Success);
        }
        else if (_AccessCreate && !string.IsNullOrEmpty(record.Username) && record.Id == 0)
        {
            ApplicationAuthenticationProviderUserMappingCreateDto applicationAuthenticationProviderUserMappingCreateDto = new ApplicationAuthenticationProviderUserMappingCreateDto()
            {                
                UserId = UserId,
                Username = record.Username
            };
            var result = await securityService.ApplicationAuthenticationProviderUserMappingCreateAsync(applicationAuthenticationProviderUserMappingCreateDto);
            if (result != null)
            {
                if (!result.HasError)
                    Snackbar.Add("User Mapping Created", Severity.Success);
                else
                    Snackbar.Add(result.Message, Severity.Error);
            }
        }
        else if (!_AccessCreate && !string.IsNullOrEmpty(record.Username) && record.Id == 0)
        {
            Snackbar.Add("You don't have permission to create the User Mapping", Severity.Success);
        }

        await LoadApplicationAuthenticationProviderUserMappings();
    }
}
