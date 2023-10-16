using System.ComponentModel.DataAnnotations;

namespace DsK.AuthServer.Security.Shared;
public partial class ApplicationUpdateDto
{
    [Key]
    public int Id { get; set; }

    public bool IsEnabled { get; set; }

    [Required]
    public string ApplicationDesc { get; set; } = null!;

    [Required]
    public string CallbackURL { get; set; } = null!;

}
