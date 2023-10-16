namespace DsK.AuthServer.Security.Shared;
public partial class UserPermissionChangeDto
{
    public int UserId { get; set; }

    public int PermissionId { get; set; }

    public bool IsEnabled { get; set; }

    public bool Overwrite { get; set; }    
}
