using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared;

public partial class ApplicationUserGridDto
{
    public int ApplicationId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool UserEnabled { get; set; }
}
