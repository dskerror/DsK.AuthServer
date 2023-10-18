using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using DsK.AuthServer.Security.Shared;
using MudBlazor;

namespace DsK.AuthServer.Client.Pages;
public partial class ApplicationAuthenticationProviderUserMappings
{
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    [Parameter] public int ApplicationId { get; set; }
    [Parameter] public int UserId { get; set; }
    public List<ApplicationAuthenticationProviderUserMappingsGridDto> model { get; set; }


    private bool _AccessView;
    private bool _loaded;
    private bool _AccessEdit;

    private List<BreadcrumbItem> _breadcrumbs;


    protected override async Task OnInitializedAsync()
    {
        var state = await authenticationState;
        SetPermissions(state);

        if (_AccessView)
        {
            _breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem("Applications", href: "Applications"),
                new BreadcrumbItem("Application View/Edit", href: $"Application/{ApplicationId}"),
                new BreadcrumbItem("Authentication Provider User Mappings", href: null, disabled: true)
            };

            await LoadApplicationAuthenticationProviderUserMappings();
        }
    }
    private void SetPermissions(AuthenticationState state)
    {
        _AccessView = securityService.HasPermission(state.User, Access.ApplicationAuthenticationProviderUserMappings.View);
        _AccessEdit = securityService.HasPermission(state.User, Access.ApplicationAuthenticationProviderUserMappings.Edit);
    }

    private async Task LoadApplicationAuthenticationProviderUserMappings()
    {
        var result = await securityService.ApplicationAuthenticationProviderUserMappingsGetAsync(ApplicationId, UserId);
        _loaded = true;
        if (result != null)
        {
            model = result.Result;
        }
    }

    private async Task ToggleIsEnabled(ChangeEventArgs e, int applicationAuthenticationProviderId)
    {
        await securityService.ApplicationAuthenticationProviderUserMappingIsEnabledToggle(applicationAuthenticationProviderId, UserId, ApplicationId, (bool)e.Value);
        await LoadApplicationAuthenticationProviderUserMappings();
        Snackbar.Add("Application Authentication Provider User Mapping Changed", Severity.Success);
    }

    private async Task SaveMappedUsername(int id, string username)
    {
        ApplicationAuthenticationProviderUpdateDto model = new() { Id = id, Username = username };
        var result = await securityService.ApplicationAuthenticationProviderUserMappingUpdate(model);
        if (result != null)
        {
            if (result.HasError)
            {
                Snackbar.Add(result.Message, Severity.Error);
            }
            else
            {
                Snackbar.Add(result.Message, Severity.Success);
            }
        }
        else
        {
            Snackbar.Add("Unknown Error Updating Username.", Severity.Error);
        }
        
    }

    //private async Task SaveUserAuthenticatonProvider(ApplicationAuthenticationProviderUserMappingsGridDto record)
    //{
    //    if (_AccessDelete && string.IsNullOrEmpty(record.Username) && record.Id != 0)
    //    {
    //        var result = await securityService.ApplicationAuthenticationProviderUserMappingDeleteAsync(record.Id);
    //        if (result != null)
    //        {
    //            if (!result.HasError)
    //                Snackbar.Add("User Mapping Deleted", Severity.Success);
    //        }
    //    }
    //    else if (!_AccessDelete && string.IsNullOrEmpty(record.Username) && record.Id != 0)
    //    {
    //        Snackbar.Add("You don't have permission to delete the User Authentication Provider", Severity.Success);
    //    }
    //    else if (_AccessEdit && !string.IsNullOrEmpty(record.Username) && record.Id != 0)
    //    {
    //        ApplicationAuthenticationProviderUserMappingUpdateDto applicationAuthenticationProviderUserMappingUpdateDto = new ApplicationAuthenticationProviderUserMappingUpdateDto()
    //        {
    //            Id = record.Id,
    //            Username = record.Username
    //        };
    //        var result = await securityService.ApplicationAuthenticationProviderUserMappingEditAsync(applicationAuthenticationProviderUserMappingUpdateDto);
    //        if (result != null)
    //        {
    //            if (!result.HasError)
    //                Snackbar.Add("User Mapping Edited", Severity.Success);
    //            else
    //                Snackbar.Add(result.Message, Severity.Error);
    //        }
    //    }
    //    else if (!_AccessEdit && !string.IsNullOrEmpty(record.Username) && record.Id != 0)
    //    {
    //        Snackbar.Add("You don't have permission to edit the User Mapping", Severity.Success);
    //    }
    //    else if (_AccessCreate && !string.IsNullOrEmpty(record.Username) && record.Id == 0)
    //    {
    //        ApplicationAuthenticationProviderUserMappingCreateDto applicationAuthenticationProviderUserMappingCreateDto = new ApplicationAuthenticationProviderUserMappingCreateDto()
    //        {
    //            UserId = UserId,
    //            Username = record.Username
    //        };
    //        var result = await securityService.ApplicationAuthenticationProviderUserMappingCreateAsync(applicationAuthenticationProviderUserMappingCreateDto);
    //        if (result != null)
    //        {
    //            if (!result.HasError)
    //                Snackbar.Add("User Mapping Created", Severity.Success);
    //            else
    //                Snackbar.Add(result.Message, Severity.Error);
    //        }
    //    }
    //    else if (!_AccessCreate && !string.IsNullOrEmpty(record.Username) && record.Id == 0)
    //    {
    //        Snackbar.Add("You don't have permission to create the User Mapping", Severity.Success);
    //    }

    //    await LoadApplicationAuthenticationProviderUserMappings();
    //}
}
