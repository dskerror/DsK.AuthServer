using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class Application
{
    public int Id { get; set; }

    public Guid ApplicationGuid { get; set; }

    public string ApplicationName { get; set; } = null!;

    public string ApplicationDesc { get; set; } = null!;

    public Guid AppApiKey { get; set; }

    public virtual ICollection<ApplicationAuthenticationProvider> ApplicationAuthenticationProviders { get; } = new List<ApplicationAuthenticationProvider>();
}
