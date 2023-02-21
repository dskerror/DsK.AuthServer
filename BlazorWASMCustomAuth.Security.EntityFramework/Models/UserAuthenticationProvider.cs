using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class UserAuthenticationProvider
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int AuthenticationProviderId { get; set; }

    public string Username { get; set; } = null!;

    public virtual AuthenticationProvider AuthenticationProvider { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
