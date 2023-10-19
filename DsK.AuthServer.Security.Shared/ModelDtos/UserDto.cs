using System.ComponentModel.DataAnnotations;

namespace DsK.AuthServer.Security.Shared;
public partial class UserDto
{
    public int Id { get; set; }    

    [Required]
    [StringLength(256, MinimumLength = 6)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(256, MinimumLength = 3)]
    public string Name { get; set; } = null!;

    public bool EmailConfirmed { get; set; }

    public int AccessFailedCount { get; set; }

    public DateTime? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public string HashedPassword { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public DateTime AccountCreatedDateTime { get; set; }

    public DateTime LastPasswordChangeDateTime { get; set; }

    public DateTime? PasswordChangeDateTime { get; set; }

    public Guid? PasswordChangeGuid { get; set; }

    public bool IsEnabled { get; set; }

    public virtual ICollection<UserLogDto> UserLogs { get; } = new List<UserLogDto>();    

    public virtual ICollection<UserPermissionDto> UserPermissions { get; } = new List<UserPermissionDto>();

    public virtual ICollection<UserRoleDto> UserRoles { get; } = new List<UserRoleDto>();
}
