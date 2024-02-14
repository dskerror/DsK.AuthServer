namespace DsK.AuthServer.Client.Services.Routes
{
    public static class UserPermissionEndpoints
    {
        public static string Get(int id)
        {
            var url = $"userpermissions?UserId={id}";            
            return url;
        }

        public static string Post = "userpermissions";
    }
}
