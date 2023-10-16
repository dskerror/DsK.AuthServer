using System.ComponentModel.DataAnnotations;

namespace DsK.AuthServer.Security.Shared;

public class LoginResponseDto
{
    public string? CallbackURL{ get; set; }
    public Guid LoginToken { get; set; }
}
