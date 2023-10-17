using System;
using System.Collections.Generic;

namespace DsK.AuthServer.Security.EntityFramework.Models;

public partial class Application
{
    public int Id { get; set; }

    public Guid ApplicationGuid { get; set; }

    public string ApplicationName { get; set; } = null!;

    public string ApplicationDesc { get; set; } = null!;

    public Guid AppApiKey { get; set; }

    public string CallbackUrl { get; set; } = null!;

    public bool IsEnabled { get; set; }

    public virtual ICollection<ApplicationAuthenticationProviderUserToken> ApplicationAuthenticationProviderUserTokens { get; set; } = new List<ApplicationAuthenticationProviderUserToken>();

    public virtual ICollection<ApplicationAuthenticationProvider> ApplicationAuthenticationProviders { get; set; } = new List<ApplicationAuthenticationProvider>();

    public virtual ICollection<ApplicationPermission> ApplicationPermissions { get; set; } = new List<ApplicationPermission>();

    public virtual ICollection<ApplicationRole> ApplicationRoles { get; set; } = new List<ApplicationRole>();

    public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();

    public virtual ICollection<UserLog> UserLogs { get; set; } = new List<UserLog>();
}
