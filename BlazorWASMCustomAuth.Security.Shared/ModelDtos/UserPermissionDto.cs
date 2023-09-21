namespace BlazorWASMCustomAuth.Security.Shared;
public partial class UserPermissionDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PermissionId { get; set; }

    public bool Allow { get; set; }

    public virtual ApplicationPermissionDto Permission { get; set; } = null!;

    public virtual UserDto User { get; set; } = null!;
}
