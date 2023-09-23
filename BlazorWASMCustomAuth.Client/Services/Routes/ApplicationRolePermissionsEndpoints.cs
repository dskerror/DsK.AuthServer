using BlazorWASMCustomAuth.Security.Shared;

namespace BlazorWASMCustomAuth.Client.Services.Routes;
public static class ApplicationRolePermissionsEndpoints
{
    public static string Get(ApplicationRolePermissionsGetDto model)
    {
        var url = $"api/ApplicationRolePermissions?ApplicationId={model.ApplicationId}&RoleId={model.ApplicationRoleId}";
        return url;
    }

    public static string Post = "api/ApplicationRolePermissions";        
}
