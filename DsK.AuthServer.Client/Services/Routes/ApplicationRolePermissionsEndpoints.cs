using DsK.AuthServer.Security.Shared;

namespace DsK.AuthServer.Client.Services.Routes;
public static class ApplicationRolePermissionsEndpoints
{
    public static string Get(int ApplicationId, int ApplicationRoleId)
    {
        var url = $"ApplicationRolePermissions?ApplicationId={ApplicationId}&ApplicationRoleId={ApplicationRoleId}";
        return url;
    }

    public static string Post = "ApplicationRolePermissions";        
}
