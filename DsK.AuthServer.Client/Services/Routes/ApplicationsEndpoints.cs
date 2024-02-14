namespace DsK.AuthServer.Client.Services.Routes
{
    public static class ApplicationsEndpoints
    {
        public static string Get(int id, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            var url = $"Application?Id={id}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy={orderBy}";
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
