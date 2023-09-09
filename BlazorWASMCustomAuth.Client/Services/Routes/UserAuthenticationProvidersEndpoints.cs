using System.Reflection.Metadata.Ecma335;

namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class UserAuthenticationProvidersEndpoints
    {
        public static string Get(int userId)
        {
            var url = $"api/UserAuthenticationProviders?UserId={userId}";
            return url;
        }

        public static string Post = "api/UserAuthenticationProviders";
        public static string Put = "api/UserAuthenticationProviders";
        public static string Delete(int id)
        {
            var url = $"api/UserAuthenticationProviders?Id={id}";
            return url;
        }

    }
}
