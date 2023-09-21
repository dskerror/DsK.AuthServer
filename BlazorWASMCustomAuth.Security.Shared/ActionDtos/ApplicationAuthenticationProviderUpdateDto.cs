using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared;
public partial class ApplicationAuthenticationProviderUpdateDto
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Domain { get; set; }

    [Required]
    public string? Username { get; set; }

    [Required]    
    public string? Password { get; set; }
}
