using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared;
public class ApplicationRoleCreateDto
{
    public int ApplicationId { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string? RoleName { get; set; }

    [StringLength(250, MinimumLength = 3)]
    public string? RoleDescription { get; set; }

}
