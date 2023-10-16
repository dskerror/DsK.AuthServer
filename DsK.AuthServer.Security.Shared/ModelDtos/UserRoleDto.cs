namespace DsK.AuthServer.Security.Shared;
public partial class UserRoleDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int RoleId { get; set; }

    public virtual ApplicationRoleDto Role { get; set; } = null!;

    public virtual UserDto User { get; set; } = null!;
}
