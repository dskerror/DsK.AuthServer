using Azure.Core;
using BlazorWASMCustomAuth.Security.Shared.Requests;
using System.Drawing.Printing;

namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class ApplicationRoleEndpoints
    {
        public static string Get(ApplicationRolePagedRequest request)
        {            
            var url = $"api/ApplicationRoles?ApplicationId={request.ApplicationId}&Id={request.Id}&pageNumber={request.PageNumber}&pageSize={request.PageSize}&searchString={request.SearchString}&orderBy={request.Orderby}";
            return url;
        }

        public static string Post = "api/roles";
        public static string Put = "api/roles";
        public static string Delete = "api/roles";
    }
}
