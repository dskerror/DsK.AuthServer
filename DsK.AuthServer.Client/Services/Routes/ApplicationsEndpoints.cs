using DsK.AuthServer.Security.Shared;

namespace DsK.AuthServer.Client.Services.Routes
{
    public static class ApplicationsEndpoints
    {
        public static string Get(PagedRequest r)
        {
            var url = $"Application?Id={r.Id}&PageNumber={r.PageNumber}&PageSize={r.PageSize}&SearchString={r.SearchString}&OrderBy={r.OrderBy}";
            return url;
        }

        public static string Post = "Application";
        public static string Put = "Application";
        public static string Delete(int id)
        {
            var url = $"Application?Id={id}";
            return url;
        }

        public static string GenerateNewAPIKey = "Application/GenerateNewAPIKey";
        public static string IsEnabledToggle = "Application/IsEnabledToggle";
    }
}
