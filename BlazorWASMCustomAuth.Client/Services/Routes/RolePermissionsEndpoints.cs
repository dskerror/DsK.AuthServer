namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class RolePermissionsEndpoints
    {
        public static string Get(int RoleId)
        {
            var url = $"api/rolepermissions?RoleId={RoleId}";            
            return url;
        }

        public static string Post = "api/rolepermissions";
        
    }
}
