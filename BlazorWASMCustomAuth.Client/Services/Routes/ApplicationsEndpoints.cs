namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class ApplicationsEndpoints
    {
        public static string Get(int id, int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/Application?Id={id}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";            
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
                
        public static string Post = "api/Application";
        public static string Put = "api/Application";
        public static string Delete = "api/Application";
    }
}
