namespace BlazorWASMCustomAuth.Security.Shared;

public partial class ApplicationRoleDto
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public string RoleDescription { get; set; } = null!;

    public virtual ICollection<ApplicationRolePermissionDto> RolePermissions { get; } = new List<ApplicationRolePermissionDto>();

    public virtual ICollection<UserRoleDto> UserRoles { get; } = new List<UserRoleDto>();
}
