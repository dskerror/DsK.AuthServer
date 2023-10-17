namespace DsK.AuthServer.Client.Services.Routes;
public static class ApplicationAuthenticationProviderUserMappingsEndpoints
{
    public static string Get(int applicationId, int userId)
    {
        var url = $"api/ApplicationAuthenticationProviderUserMappings?ApplicationId={applicationId}&UserId={userId}";
        return url;
    }

    public static string IsEnabledToggle = "api/ApplicationAuthenticationProviderUserMappings/IsEnabledToggle";

    public static string Put = "api/ApplicationAuthenticationProviderUserMappings";    
}
