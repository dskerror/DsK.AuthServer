using DsK.AuthServer.Security.Shared;

namespace DsK.AuthServer.Client.Services.Routes;
public static class ApplicationUserEndpoints
{
    public static string Get(int applicationId)
    {
        var url = $"api/ApplicationUsers?ApplicationId={applicationId}";
        return url;
    }
}
