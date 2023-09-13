using Azure.Core;
using BlazorWASMCustomAuth.Security.Shared.Requests;

namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class ApplicationAuthenticationProvidersEndpoints
    {
        public static string Get(ApplicationPagedRequest request)
        {
            var url = $"api/ApplicationAuthenticationProviders?ApplicationId={request.ApplicationId}&Id={request.Id}&pageNumber={request.PageNumber}&pageSize={request.PageSize}&searchString={request.SearchString}&orderBy={request.Orderby}";
            return url;
        }
                
        public static string Post = "api/ApplicationAuthenticationProviders";
        public static string Put = "api/ApplicationAuthenticationProviders";
        public static string Delete = "api/ApplicationAuthenticationProviders";
    }
}
