namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class ApplicationsEndpoints
    {
        public static string Get(int id, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            var url = $"api/Application?Id={id}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy={orderBy}";
            return url;
        }

        public static string Post = "api/Application";
        public static string Put = "api/Application";
        public static string Delete(int id)
        {
            var url = $"api/Application?Id={id}";
            return url;
        }

        public static string GenerateNewAPIKey = "api/Application/GenerateNewAPIKey";
        public static string IsEnabledToggle = "api/Application/IsEnabledToggle";
    }
}
