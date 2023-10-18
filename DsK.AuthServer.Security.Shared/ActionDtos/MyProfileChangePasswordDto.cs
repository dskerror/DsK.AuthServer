using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DsK.AuthServer.Security.Shared;
public partial class MyProfileChangePasswordDto
{
    [Required]
    [StringLength(256, MinimumLength = 6)]
    [PasswordPropertyText]
    public string? Password { get; set; }
}
    
