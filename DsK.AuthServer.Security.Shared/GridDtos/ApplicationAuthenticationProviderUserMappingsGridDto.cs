namespace DsK.AuthServer.Security.Shared;
public partial class ApplicationAuthenticationProviderUserMappingsGridDto
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }    

    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public int ApplicationAuthenticationProviderId { get; set; }

    public string AuthenticationProviderName { get; set; } = null!;

    public string AuthenticationProviderType { get; set; } = null!;

    public string Username { get; set; } = null!;

    public bool IsEnabled { get; set; }
}
