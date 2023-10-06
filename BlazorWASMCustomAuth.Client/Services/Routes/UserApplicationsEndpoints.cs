namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class UserApplicationsEndpoints
    {
        public static string Get(int userid)
        {
            var url = $"api/UserApplications?userId={userid}";
            return url;
        }

        public static string Post = "api/UserApplications";
    }
}
