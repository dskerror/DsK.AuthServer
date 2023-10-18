using DsK.AuthServer.Security.Shared;

namespace DsK.AuthServer.Client.Services.Routes;
public static class MyProfileEndpoints
{
    public static string Get()
    {
        var url = $"api/MyProfile";
        return url;
    }
    public static string Put = "api/MyProfile";
    public static string ChangePassword = "api/MyProfile/ChangePassword";
}
