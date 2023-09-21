using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared;
public partial class ApplicationAuthenticationProviderCreateDto
{
    public int ApplicationId { get; set; }

    [Required]
    public string AuthenticationProviderName { get; set; } = null!;

    [Required]
    public string? AuthenticationProviderType { get; set; }

    public string? Domain { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }
}
