using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared;

public partial class AuthenticationProviderUpdateDto
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string AuthenticationProviderName { get; set; } = null!;

    [Required]
    public string? AuthenticationProviderType { get; set; }

    public string? Domain { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }
}
