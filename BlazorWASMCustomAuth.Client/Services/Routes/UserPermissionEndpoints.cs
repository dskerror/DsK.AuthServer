namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class UserPermissionEndpoints
    {
        public static string Get(int id)
        {
            var url = $"api/security/userpermissions?UserId={id}";            
            return url;
        }

        public static string Post = "api/security/userpermissions";
    }
}
