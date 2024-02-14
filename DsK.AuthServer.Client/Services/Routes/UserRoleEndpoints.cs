namespace DsK.AuthServer.Client.Services.Routes
{
    public static class UserRolesEndpoints
    {
        public static string Get(int id)
        {
            var url = $"userroles?UserId={id}";            
            return url;
        }

        public static string Post = "userroles";
    }
}
