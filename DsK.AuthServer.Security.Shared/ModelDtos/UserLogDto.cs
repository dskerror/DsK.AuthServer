namespace DsK.AuthServer.Security.Shared;
public partial class UserLogDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime EventDateTime { get; set; }

    public string Event { get; set; } = null!;

    public virtual UserDto User { get; set; } = null!;
}
