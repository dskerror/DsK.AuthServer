namespace BlazorWASMCustomAuth.Security.Shared;

public partial class ApplicationRolePermissionChangeDto
{    
    public int ApplicationRoleId { get; set; }

    public int ApplicationPermissionId { get; set; }

    public bool IsEnabled { get; set; }
}
