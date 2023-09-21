using BlazorWASMCustomAuth.Security.Shared;

namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class ApplicationPermissionEndpoints
    {        public static string Get(ApplicationPagedRequest request)
        {
            var url = $"api/ApplicationPermissions?ApplicationId={request.ApplicationId}&Id={request.Id}&pageNumber={request.PageNumber}&pageSize={request.PageSize}&searchString={request.SearchString}&orderBy={request.Orderby}";
            return url;
        }

        public static string Post = "api/ApplicationPermissions";
        public static string Put = "api/ApplicationPermissions";
        public static string Delete = "api/ApplicationPermissions";
    }
}
