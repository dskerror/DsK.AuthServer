namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class RoleEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/security/roles/GetAllPaged?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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

        public static string GetCount = "api/security/roles/count";
        public static string Save = "api/security/roles";
        public static string Delete = "api/security/roles";
        
    }
}
