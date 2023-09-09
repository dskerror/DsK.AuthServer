namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class UserEndpoints
    {
        public static string Get(int id, int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/users?Id={id}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
            if (orderBy?.Any() == true)
            {
                foreach (var orderByPart in orderBy)
                {
                    url += $"{orderByPart},";
                }
                url = url[..^1]; // loose training ,
            }
            return url;
        }

        public static string Post = "api/users";
        public static string Put = "api/users";
    }
}
