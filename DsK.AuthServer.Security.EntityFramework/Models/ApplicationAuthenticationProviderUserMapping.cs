using System;
using System.Collections.Generic;

namespace DsK.AuthServer.Security.EntityFramework.Models;

public partial class ApplicationAuthenticationProviderUserMapping
{
    public int Id { get; set; }

    public int ApplicationAuthenticationProviderId { get; set; }

    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public bool IsEnabled { get; set; }

    public virtual ApplicationAuthenticationProvider ApplicationAuthenticationProvider { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
