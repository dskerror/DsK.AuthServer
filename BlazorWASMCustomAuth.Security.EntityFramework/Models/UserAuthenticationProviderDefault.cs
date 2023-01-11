using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class UserAuthenticationProviderDefault
{
    public int UserId { get; set; }

    public int AuthenticationProviderId { get; set; }
}
