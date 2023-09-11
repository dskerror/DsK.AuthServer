﻿using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class UserAuthenticationProviderMapping
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ApplicationAuthenticationProviderId { get; set; }

    public string Username { get; set; } = null!;

    public virtual ApplicationAuthenticationProvider ApplicationAuthenticationProvider { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
