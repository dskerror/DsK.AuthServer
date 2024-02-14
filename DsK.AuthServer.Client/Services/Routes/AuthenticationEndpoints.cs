namespace DsK.AuthServer.Client.Services.Routes
{
    public static class AuthenticationEndpoints
    {
        public static string Login = "Authentication/Login";
        public static string ValidateLoginToken = "Authentication/ValidateLoginToken";
        public static string RefreshToken = "Authentication/RefreshToken";
        public static string Register = "Authentication/Register";
        public static string PasswordChangeRequest = "Authentication/PasswordChangeRequest";
        public static string PasswordChange = "Authentication/PasswordChange";
        public static string EmailConfirm = "Authentication/EmailConfirm";
    }
}
