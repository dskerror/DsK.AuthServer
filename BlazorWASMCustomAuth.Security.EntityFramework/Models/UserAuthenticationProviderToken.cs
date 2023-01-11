using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class UserAuthenticationProviderToken
{
    public int Id { get; set; }

    public int UserAuthenticationProviderId { get; set; }

    public string Token { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public DateTime TokenCreatedDateTime { get; set; }

    public virtual UserAuthenticationProvider UserAuthenticationProvider { get; set; } = null!;
}
