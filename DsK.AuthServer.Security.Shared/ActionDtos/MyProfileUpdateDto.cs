using System.ComponentModel.DataAnnotations;

namespace DsK.AuthServer.Security.Shared;
public partial class MyProfileUpdateDto
{
    [Required]
    [StringLength(256, MinimumLength = 3)]
    public string Name { get; set; } = null!;
}
    
