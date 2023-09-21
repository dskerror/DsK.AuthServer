namespace BlazorWASMCustomAuth.Security.Shared;
public partial class UserPermissionChangeDto
{
    public int UserId { get; set; }

    public int PermissionId { get; set; }

    public bool Enabled { get; set; }

    public bool Allow { get; set; }    
}
