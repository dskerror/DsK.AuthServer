namespace DsK.AuthServer.Security.Shared;
public partial class UserPermissionGridDto
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public string ApplicationName { get; set; }
    public string PermissionName { get; set; }
    public string PermissionDescription { get; set; }
    public bool Overwrite { get; set; }
    public bool IsEnabled { get; set; }
    public string Roles { get; set; }
}
