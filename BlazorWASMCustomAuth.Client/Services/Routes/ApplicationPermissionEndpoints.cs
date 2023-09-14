namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class ApplicationPermissionEndpoints
    {
        public static string Get(int applicationId)
        {
            var url = $"api/ApplicationPermissions?ApplicationId={applicationId}";            
            return url;
        }
    }
}
