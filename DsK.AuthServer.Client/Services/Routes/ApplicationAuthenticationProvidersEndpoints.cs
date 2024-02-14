using Azure.Core;
using DsK.AuthServer.Security.Shared;

namespace DsK.AuthServer.Client.Services.Routes;
public static class ApplicationAuthenticationProvidersEndpoints
{
    public static string Get(ApplicationPagedRequest request)
    {
        var url = $"ApplicationAuthenticationProviders?ApplicationId={request.ApplicationId}&Id={request.Id}&pageNumber={request.PageNumber}&pageSize={request.PageSize}&searchString={request.SearchString}&orderBy={request.Orderby}";
        return url;
    }
    
    public static string Post = "ApplicationAuthenticationProviders";
    public static string Put = "ApplicationAuthenticationProviders";
    public static string IsEnabledToggle = "ApplicationAuthenticationProviders/IsEnabledToggle";
    public static string ValidateDomainConnection = "ApplicationAuthenticationProviders/ValidateDomainConnection";
    
    public static string ValidateApplicationAuthenticationProviderGuid(string ApplicationAuthenticationProviderGuid)
    {
        var url = $"ApplicationAuthenticationProviders/ValidateApplicationAuthenticationProviderGuid?ApplicationAuthenticationProviderGuid={ApplicationAuthenticationProviderGuid}";
        return url;
    }

    public static string Delete(int id)
    {
        var url = $"ApplicationAuthenticationProviders?Id={id}";
        return url;
    }
}
