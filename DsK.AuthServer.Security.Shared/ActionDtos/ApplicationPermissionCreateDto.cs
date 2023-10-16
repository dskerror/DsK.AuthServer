using System.ComponentModel.DataAnnotations;

namespace DsK.AuthServer.Security.Shared;
public class ApplicationPermissionCreateDto
{
    public int ApplicationId { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string? PermissionName { get; set; }

    [Required]
    [StringLength(250, MinimumLength = 3)]
    public string? PermissionDescription { get; set; }
}
