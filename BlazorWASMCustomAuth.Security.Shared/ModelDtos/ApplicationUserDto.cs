namespace BlazorWASMCustomAuth.Security.Shared;

public partial class ApplicationUserDto
{
    public int Id { get; set; }

    public int ApplicationId { get; set; }

    public int UserId { get; set; }

    public DateTime? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public bool UserEnabled { get; set; }

    public virtual ApplicationDto Application { get; set; } = null!;

    public virtual UserDto User { get; set; } = null!;
}
