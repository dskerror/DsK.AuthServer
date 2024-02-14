namespace DsK.AuthServer.Client.Services.Routes
{
    public static class UserApplicationsEndpoints
    {
        public static string Get(int userid)
        {
            var url = $"UserApplications?userId={userid}";
            return url;
        }

        public static string Post = "UserApplications";
    }
}
