using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared;
public partial class MyProfileChangePasswordDto
{
    public int UserId { get; set; }

    [Required]
    [StringLength(256, MinimumLength = 6)]
    [PasswordPropertyText]
    public string? Password { get; set; }
}
    
