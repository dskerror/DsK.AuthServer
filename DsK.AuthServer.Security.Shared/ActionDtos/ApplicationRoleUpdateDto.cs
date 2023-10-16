using System.ComponentModel.DataAnnotations;

namespace DsK.AuthServer.Security.Shared;
public class ApplicationRoleUpdateDto
{
    public int Id { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string? RoleName { get; set; }

    [Required]
    [StringLength(250, MinimumLength = 3)]
    public string? RoleDescription { get; set; }
}
