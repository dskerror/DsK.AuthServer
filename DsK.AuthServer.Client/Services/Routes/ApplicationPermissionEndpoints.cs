using DsK.AuthServer.Security.Shared;

namespace DsK.AuthServer.Client.Services.Routes
{
    public static class ApplicationPermissionEndpoints
    {        public static string Get(ApplicationPagedRequest request)
        {
            var url = $"ApplicationPermissions?ApplicationId={request.ApplicationId}&Id={request.Id}&pageNumber={request.PageNumber}&pageSize={request.PageSize}&searchString={request.SearchString}&orderBy={request.Orderby}";
            return url;
        }

        public static string Post = "ApplicationPermissions";
        public static string Put = "ApplicationPermissions";
        public static string IsEnabledToggle = "ApplicationPermissions/IsEnabledToggle";

        public static string Delete(int id)
        {
            var url = $"ApplicationPermissions?Id={id}";
            return url;
        }
    }
}
