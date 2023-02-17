namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class AuthenticationEndpoints
    {
        public static string Login = "/api/security/Authentication/userlogin";
        public static string RefreshToken = "/api/security/Authentication/RefreshToken";
    }
}
