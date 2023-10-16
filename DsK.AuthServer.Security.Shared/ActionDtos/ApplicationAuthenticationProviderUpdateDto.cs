using System.ComponentModel.DataAnnotations;

namespace DsK.AuthServer.Security.Shared;
public partial class ApplicationAuthenticationProviderUpdateDto
{    
    [Key]
    public int Id { get; set; }

    public bool IsEnabled { get; set; }

    public Guid ApplicationAuthenticationProviderGUID { get; set; }

    [Required]
    public string? Domain { get; set; }

    [Required]
    public string? Username { get; set; }

    [Required]    
    public string? Password { get; set; }

    public bool RegistrationEnabled { get; set; }

    public bool RegistrationAutoEmailConfirm { get; set; }

    public bool ActiveDirectoryFirstLoginAutoRegister { get; set; }
}
