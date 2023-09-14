namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class UserEndpoints
    {
        public static string Get(int id, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            var url = $"api/users?Id={id}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy={orderBy}";
            return url;
        }

        public static string Post = "api/users";
        public static string Put = "api/users";
    }
}
