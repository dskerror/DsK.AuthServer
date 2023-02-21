using System.Reflection.Metadata.Ecma335;

namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class UserAuthenticationProvidersEndpoints
    {
        public static string Get(int userId)
        {
            var url = $"api/security/UserAuthenticationProviders?UserId={userId}";
            return url;
        }

        public static string Post = "api/security/UserAuthenticationProviders";
        public static string Put = "api/security/UserAuthenticationProviders";
        public static string Delete(int id)
        {
            var url = $"api/security/UserAuthenticationProviders?Id={id}";
            return url;
        }

    }
}
