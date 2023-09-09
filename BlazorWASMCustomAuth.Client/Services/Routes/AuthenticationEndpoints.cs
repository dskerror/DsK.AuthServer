namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class AuthenticationEndpoints
    {
        public static string Login = "/api/Authentication/userlogin";
        public static string RefreshToken = "/api/Authentication/RefreshToken";
    }
}
