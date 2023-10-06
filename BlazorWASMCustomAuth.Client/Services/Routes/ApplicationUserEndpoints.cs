using BlazorWASMCustomAuth.Security.Shared;

namespace BlazorWASMCustomAuth.Client.Services.Routes;
public static class ApplicationUserEndpoints
{
    public static string Get(int applicationId)
    {
        var url = $"api/ApplicationUsers?ApplicationId={applicationId}";
        return url;
    }
}
