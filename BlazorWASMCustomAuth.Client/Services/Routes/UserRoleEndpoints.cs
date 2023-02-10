namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class UserRolesEndpoints
    {
        public static string Get(int id)
        {
            var url = $"api/security/userroles?UserId={id}";            
            return url;
        }

        public static string Post = "api/security/userroles";
    }
}
