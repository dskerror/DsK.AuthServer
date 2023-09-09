namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class UserRolesEndpoints
    {
        public static string Get(int id)
        {
            var url = $"api/userroles?UserId={id}";            
            return url;
        }

        public static string Post = "api/userroles";
    }
}
