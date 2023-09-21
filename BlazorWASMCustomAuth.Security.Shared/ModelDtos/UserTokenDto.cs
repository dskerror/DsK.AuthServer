namespace BlazorWASMCustomAuth.Security.Shared;
public partial class UserTokenDto
{
    public int Id { get; set; }

    public string Token { get; set; } = null!;

    public int UserId { get; set; }

    public string RefreshToken { get; set; } = null!;

    public DateTime TokenCreatedDateTime { get; set; }

    public DateTime TokenRefreshedDateTime { get; set; }
}
