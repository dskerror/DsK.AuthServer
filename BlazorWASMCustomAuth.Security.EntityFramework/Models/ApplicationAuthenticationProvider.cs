using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class ApplicationAuthenticationProvider
{
    public int Id { get; set; }

    public int ApplicationId { get; set; }

    public int AuthenticationProviderId { get; set; }

    public string? Domain { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public virtual Application Application { get; set; } = null!;

    public virtual AuthenticationProvider AuthenticationProvider { get; set; } = null!;

    public virtual ICollection<UserAuthenticationProviderMapping> UserAuthenticationProviderMappings { get; } = new List<UserAuthenticationProviderMapping>();
}
