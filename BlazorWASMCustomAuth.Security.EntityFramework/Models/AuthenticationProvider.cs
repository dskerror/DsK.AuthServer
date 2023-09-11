using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class AuthenticationProvider
{
    public int Id { get; set; }

    public string AuthenticationProviderName { get; set; } = null!;

    public string? AuthenticationProviderType { get; set; }

    public virtual ICollection<ApplicationAuthenticationProvider> ApplicationAuthenticationProviders { get; } = new List<ApplicationAuthenticationProvider>();

    public virtual ICollection<UserAuthenticationProviderMapping> UserAuthenticationProviderMappings { get; } = new List<UserAuthenticationProviderMapping>();
}
