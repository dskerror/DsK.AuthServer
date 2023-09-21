namespace BlazorWASMCustomAuth.Security.Shared;
public partial class UserPasswordDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string HashedPassword { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public virtual UserDto User { get; set; } = null!;
}
