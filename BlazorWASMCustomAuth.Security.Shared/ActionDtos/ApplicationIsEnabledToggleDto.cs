using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared;
public partial class ApplicationIsEnabledToggleDto
{
    [Key]
    public int Id { get; set; }
}
