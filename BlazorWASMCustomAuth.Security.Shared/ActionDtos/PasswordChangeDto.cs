using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared;

public class PasswordChangeDto
{
    [Required]
    [StringLength(256, MinimumLength = 6)]
    [PasswordPropertyText]
    public string? Password { get; set; }
    public Guid PasswordChangeGuid { get; set; }
}
