namespace BlazorWASMCustomAuth.Security.Shared;
public partial class ApplicationAuthenticationProviderDto
{
    public int Id { get; set; }

    public int ApplicationId { get; set; }

    public string Name { get; set; } = null!;

    public string? AuthenticationProviderType { get; set; }

    public string? Domain { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }
}
