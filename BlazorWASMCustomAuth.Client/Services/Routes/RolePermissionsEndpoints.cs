namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class RolePermissionsEndpoints
    {
        public static string Get(int RoleId)
        {
            var url = $"api/security/rolepermissions?RoleId={RoleId}";            
            return url;
        }

        public static string Post = "api/security/rolepermissions";
        
    }
}
