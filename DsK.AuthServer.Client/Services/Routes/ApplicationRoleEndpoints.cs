using DsK.AuthServer.Security.Shared;

namespace DsK.AuthServer.Client.Services.Routes;
public static class ApplicationRoleEndpoints
{
    public static string Get(ApplicationPagedRequest request)
    {            
        var url = $"api/ApplicationRoles?ApplicationId={request.ApplicationId}&Id={request.Id}&pageNumber={request.PageNumber}&pageSize={request.PageSize}&searchString={request.SearchString}&orderBy={request.Orderby}";
        return url;
    }

    public static string Post = "api/ApplicationRoles";
    public static string Put = "api/ApplicationRoles";
    public static string IsEnabledToggle = "api/ApplicationRoles/IsEnabledToggle";

    public static string Delete(int id)
    {
        var url = $"api/ApplicationRoles?Id={id}";
        return url;
    }
}
