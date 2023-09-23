using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared;

public class LoginResponseDto
{
    public string CallbackURL { get; set; }
}
