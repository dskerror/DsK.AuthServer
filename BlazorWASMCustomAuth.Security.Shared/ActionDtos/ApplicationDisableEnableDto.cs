using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared;
public partial class ApplicationDisableEnableDto
{
    [Key]
    public int Id { get; set; }
}
