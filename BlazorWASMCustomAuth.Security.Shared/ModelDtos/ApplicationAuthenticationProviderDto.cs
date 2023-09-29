namespace BlazorWASMCustomAuth.Security.Shared;
public partial class ApplicationAuthenticationProviderDto
{
    public int Id { get; set; }

    public Guid ApplicationAuthenticationProviderGUID { get; set; }

    public int ApplicationId { get; set; }

    public int? DefaultApplicationRoleId { get; set; }

    public string? AuthenticationProviderType { get; set; }

    public string Name { get; set; } = null!;

    public string? Domain { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public bool ApplicationAuthenticationProviderDisabled { get; set; }

    public bool RegistrationEnabled { get; set; }

    public bool RegistrationAutoEmailConfirm { get; set; }
}
