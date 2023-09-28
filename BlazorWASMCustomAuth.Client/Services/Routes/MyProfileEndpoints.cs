using BlazorWASMCustomAuth.Security.Shared;

namespace BlazorWASMCustomAuth.Client.Services.Routes;
public static class MyProfileEndpoints
{
    public static string Get(int id)
    {
        var url = $"api/MyProfile?Id={id}";
        return url;
    }
    public static string Put = "api/MyProfile";
    public static string ChangePassword = "api/MyProfile/ChangePassword";
}
