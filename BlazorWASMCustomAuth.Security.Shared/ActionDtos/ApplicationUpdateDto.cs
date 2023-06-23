using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared;

public partial class ApplicationUpdateDto
{
    [Key]
    public int Id { get; set; }

    [Required]

    public string ApplicationDesc { get; set; } = null!;
}
