using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class ApplicationAuthenticationProviderLogin
{
    public int Id { get; set; }

    public int ApplicationAuthenticationProviderId { get; set; }

    public Guid LoginKey { get; set; }

    public DateTime DateTimeGenerated { get; set; }

    public virtual ApplicationAuthenticationProvider ApplicationAuthenticationProvider { get; set; } = null!;
}
