using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BlazorWASMCustomAuth.Security.Shared;
public class UserAuthenticationProviderCreateDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int AuthenticationProviderId { get; set; }

    [Required]
    [StringLength(256, MinimumLength = 3)]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Username can't contain spaces.")]
    [DefaultValue("jsmith")]
    public string? Username { get; set; }
}
