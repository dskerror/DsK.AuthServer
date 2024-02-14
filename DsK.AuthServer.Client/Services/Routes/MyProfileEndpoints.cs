using DsK.AuthServer.Security.Shared;

namespace DsK.AuthServer.Client.Services.Routes;
public static class MyProfileEndpoints
{
    public static string Get()
    {
        var url = $"MyProfile";
        return url;
    }
    public static string Put = "MyProfile";
    public static string ChangePassword = "MyProfile/ChangePassword";
}
