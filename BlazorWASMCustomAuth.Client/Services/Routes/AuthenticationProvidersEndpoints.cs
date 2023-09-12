namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class AuthenticationProvidersEndpoints
    {
        public static string Get(int id, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            var url = $"api/AuthenticationProviders?Id={id}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy={orderBy}";
            return url;
        }
                
        public static string Post = "api/AuthenticationProviders";
        public static string Put = "api/AuthenticationProviders";
        public static string Delete = "api/AuthenticationProviders";
    }
}
