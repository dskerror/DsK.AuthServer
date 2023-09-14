namespace BlazorWASMCustomAuth.Client.Services.Routes;
public static class ApplicationRolePermissionsEndpoints
{
    public static string Get(int applicationId, int applicationRoleId)
    {
        var url = $"api/ApplicationRolePermissions?ApplicationId={applicationId}&RoleId={applicationRoleId}";
        return url;
    }

    public static string Post = "api/ApplicationRolePermissions";        
}
