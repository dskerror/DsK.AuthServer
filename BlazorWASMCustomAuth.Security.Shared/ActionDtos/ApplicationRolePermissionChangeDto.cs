namespace BlazorWASMCustomAuth.Security.Shared;

public partial class ApplicationRolePermissionChangeDto
{    
    public int RoleId { get; set; }

    public int PermissionId { get; set; }

    public bool IsEnabled { get; set; }
}
