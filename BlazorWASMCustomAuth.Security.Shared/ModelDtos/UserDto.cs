using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared;
public partial class UserDto
{
    public int Id { get; set; }

   
    public string Username { get; set; } = null!;

    [Required]
    [StringLength(256, MinimumLength = 3)]
    public string Name { get; set; } = null!;
    [Required]
    [StringLength(256, MinimumLength = 6)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public bool EmailConfirmed { get; set; }

    public DateTime? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public virtual ICollection<UserLogDto> UserLogs { get; } = new List<UserLogDto>();

    public virtual ICollection<UserPasswordDto> UserPasswords { get; } = new List<UserPasswordDto>();

    public virtual ICollection<UserPermissionDto> UserPermissions { get; } = new List<UserPermissionDto>();

    public virtual ICollection<UserRoleDto> UserRoles { get; } = new List<UserRoleDto>();
}
