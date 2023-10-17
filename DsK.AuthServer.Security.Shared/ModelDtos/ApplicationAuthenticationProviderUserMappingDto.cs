namespace DsK.AuthServer.Security.Shared;
public class ApplicationAuthenticationProviderUserMappingDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int AuthenticationProviderId { get; set; }

    public string Username { get; set; } = null!;

    public virtual ApplicationAuthenticationProviderDto AuthenticationProvider { get; set; } = null!;

    public virtual UserDto User { get; set; } = null!;
}
