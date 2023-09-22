using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared;
public partial class ApplicationCreateDto
{
    [Required]
    public string ApplicationName { get; set; } = null!;

    public string ApplicationDesc { get; set; } = null!;

    [Required]
    public string CallbackURL { get; set; } = null!;
}
