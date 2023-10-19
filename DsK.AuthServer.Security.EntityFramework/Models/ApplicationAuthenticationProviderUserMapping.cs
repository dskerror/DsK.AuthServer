using System;
using System.Collections.Generic;

namespace DsK.AuthServer.Security.EntityFramework.Models;

public partial class ApplicationAuthenticationProviderUserMapping
{
    public int Id { get; set; }

    public int ApplicationAuthenticationProviderId { get; set; }

    public int ApplicationUserId { get; set; }

    public string Username { get; set; } = null!;

    public bool IsEnabled { get; set; }

    public virtual ApplicationAuthenticationProvider ApplicationAuthenticationProvider { get; set; } = null!;

    public virtual ApplicationUser ApplicationUser { get; set; } = null!;
}
