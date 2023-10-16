namespace DsK.AuthServer.Security.Shared;
public class UserRoleChangeDto
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public bool IsEnabled { get; set; }
}
