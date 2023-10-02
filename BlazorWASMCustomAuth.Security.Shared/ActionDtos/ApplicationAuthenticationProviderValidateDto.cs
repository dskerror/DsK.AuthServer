namespace BlazorWASMCustomAuth.Security.Shared;
public partial class ApplicationAuthenticationProviderValidateDto
{   
    public int Id { get; set; }
    public Guid ApplicationAuthenticationProviderGUID { get; set; }

    public string? AuthenticationProviderType { get; set; }

    public bool ApplicationAuthenticationProviderDisabled { get; set; }

    public bool RegistrationEnabled { get; set; }

    public bool RegistrationAutoEmailConfirm { get; set; }

    public bool ActiveDirectoryFirstLoginAutoRegister { get; set; }
}