using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class AuthenticationProvider
{
    public int Id { get; set; }

    public string AuthenticationProviderName { get; set; } = null!;

    public virtual ICollection<UserAuthenticationProvider> UserAuthenticationProviders { get; } = new List<UserAuthenticationProvider>();
}
