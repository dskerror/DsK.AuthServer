using BlazorWASMCustomAuth.Security.Shared.Requests;
using System.Reflection.Metadata.Ecma335;

namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class UserAuthenticationProvidersEndpoints
    {
        public static string Get(int userId)
        {
            var url = ""//$"api/UserAuthenticationProviders?ApplicationId={request.ApplicationId}?Id={request.Id}&pageNumber={request.PageNumber}&pageSize={request.PageSize}&searchString={request.SearchString}&orderBy={request.Orderby}";
            return url;
        }

        public static string Post = "api/UserAuthenticationProviders";
        public static string Put = "api/UserAuthenticationProviders";
        public static string Delete(int id)
        {
            var url = $"api/UserAuthenticationProviders?Id={id}";
            return url;
        }
    }
}
