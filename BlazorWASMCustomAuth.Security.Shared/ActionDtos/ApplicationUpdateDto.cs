using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared;
public partial class ApplicationUpdateDto
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string ApplicationDesc { get; set; } = null!;

    [Required]
    public string CallbackURL { get; set; }
    public bool ApplicationDisabled { get; set; }
}
