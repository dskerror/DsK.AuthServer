using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared;

public class UserLoginDto
{
    [Required]
    [StringLength(256, MinimumLength = 5)]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
    public Guid ApplicationAuthenticationProviderGUID  { get; set; }
}
