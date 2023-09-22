namespace BlazorWASMCustomAuth.Security.Shared;
public partial class ApplicationPermissionDto
{
    public int Id { get; set; }

    public int ApplicationId { get; set; }
    public string PermissionName { get; set; } = null!;

    public string PermissionDescription { get; set; } = null!;

    public bool PermissionDisabled { get; set; }

    public virtual ICollection<ApplicationRolePermissionDto> RolePermissions { get; } = new List<ApplicationRolePermissionDto>();

    public virtual ICollection<UserPermissionDto> UserPermissions { get; } = new List<UserPermissionDto>();
}
