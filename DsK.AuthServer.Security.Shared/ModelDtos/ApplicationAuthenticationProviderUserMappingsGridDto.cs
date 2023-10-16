namespace DsK.AuthServer.Security.Shared;
public partial class ApplicationAuthenticationProviderUserMappingsGridDto
{
    public int Id { get; set; }

    public int ApplicationId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string AuthenticationProviderType { get; set; } = null!;

    public string Username { get; set; } = null!;

}
