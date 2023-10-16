using Azure.Core;
using DsK.AuthServer.Security.Shared;

namespace DsK.AuthServer.Client.Services.Routes;
public static class ApplicationAuthenticationProvidersEndpoints
{
    public static string Get(ApplicationPagedRequest request)
    {
        var url = $"api/ApplicationAuthenticationProviders?ApplicationId={request.ApplicationId}&Id={request.Id}&pageNumber={request.PageNumber}&pageSize={request.PageSize}&searchString={request.SearchString}&orderBy={request.Orderby}";
        return url;
    }
    
    public static string Post = "api/ApplicationAuthenticationProviders";
    public static string Put = "api/ApplicationAuthenticationProviders";
    public static string IsEnabledToggle = "api/ApplicationAuthenticationProviders/IsEnabledToggle";
    public static string ValidateDomainConnection = "api/ApplicationAuthenticationProviders/ValidateDomainConnection";
    
    public static string ValidateApplicationAuthenticationProviderGuid(string ApplicationAuthenticationProviderGuid)
    {
        var url = $"api/ApplicationAuthenticationProviders/ValidateApplicationAuthenticationProviderGuid?ApplicationAuthenticationProviderGuid={ApplicationAuthenticationProviderGuid}";
        return url;
    }

    public static string Delete(int id)
    {
        var url = $"api/ApplicationAuthenticationProviders?Id={id}";
        return url;
    }
}
