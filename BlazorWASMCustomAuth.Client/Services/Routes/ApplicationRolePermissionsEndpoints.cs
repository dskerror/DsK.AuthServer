using BlazorWASMCustomAuth.Security.Shared;

namespace BlazorWASMCustomAuth.Client.Services.Routes;
public static class ApplicationRolePermissionsEndpoints
{
    public static string Get(int ApplicationId, int ApplicationRoleId)
    {
        var url = $"api/ApplicationRolePermissions?ApplicationId={ApplicationId}&ApplicationRoleId={ApplicationRoleId}";
        return url;
    }

    public static string Post = "api/ApplicationRolePermissions";        
}
