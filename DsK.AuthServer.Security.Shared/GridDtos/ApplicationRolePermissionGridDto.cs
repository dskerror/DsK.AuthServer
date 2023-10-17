namespace DsK.AuthServer.Security.Shared;
public partial class ApplicationRolePermissionGridDto
{   
    public int Id { get; set; }
    public string PermissionName { get; set; }
    public string PermissionDescription { get; set; }    
    public bool Overwrite { get; set; }
}
