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
        public static string Delete = "api/Application";
        public static string GenerateNewAPIKey = "api/Application/GenerateNewAPIKey";
    }
}
