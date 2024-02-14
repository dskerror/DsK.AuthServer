using DsK.AuthServer.Security.Shared;

namespace DsK.AuthServer.Client.Services.Routes;
public static class ApplicationRoleEndpoints
{
    public static string Get(ApplicationPagedRequest request)
    {            
        var url = $"ApplicationRoles?ApplicationId={request.ApplicationId}&Id={request.Id}&pageNumber={request.PageNumber}&pageSize={request.PageSize}&searchString={request.SearchString}&orderBy={request.Orderby}";
        return url;
    }

    public static string Post = "ApplicationRoles";
    public static string Put = "ApplicationRoles";
    public static string IsEnabledToggle = "ApplicationRoles/IsEnabledToggle";

    public static string Delete(int id)
    {
        var url = $"ApplicationRoles?Id={id}";
        return url;
    }
}
