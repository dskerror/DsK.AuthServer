using BlazorWASMCustomAuth.Security.Shared.Requests;

namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class ApplicationUserEndpoints
    {
        public static string Get(ApplicationPagedRequest request)
        {
            var url = $"api/ApplicationUsers?applicationId={request.ApplicationId}&" +
                $"Id={request.Id}&" +
                $"pageNumber={request.PageNumber}&" + 
                $"pageSize={request.PageSize}&" +
                $"searchString={request.SearchString}&" +
                $"orderBy={request.Orderby}";
            return url;
        }

        public static string Post = "api/ApplicationUsers";
        public static string Put = "api/ApplicationUsers";
    }
}
