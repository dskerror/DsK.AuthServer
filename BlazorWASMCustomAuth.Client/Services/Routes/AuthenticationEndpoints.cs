namespace BlazorWASMCustomAuth.Client.Services.Routes
{
    public static class AuthenticationEndpoints
    {
        public static string Login = "/api/Authentication/Login";
        public static string ValidateLoginToken = "/api/Authentication/ValidateLoginToken";
        public static string RefreshToken = "/api/Authentication/RefreshToken";
        public static string Register = "/api/Authentication/Register";
        public static string PasswordChangeRequest = "/api/Authentication/PasswordChangeRequest";
        public static string PasswordChange = "/api/Authentication/PasswordChange";
        public static string EmailConfirm = "/api/Authentication/EmailConfirm";
    }
}
