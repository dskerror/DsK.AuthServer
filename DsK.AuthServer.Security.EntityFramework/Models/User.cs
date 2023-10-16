using System;
using System.Collections.Generic;

namespace DsK.AuthServer.Security.EntityFramework.Models;

public partial class User
{
    public int Id { get; set; }

    public bool IsEnabled { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool EmailConfirmed { get; set; }

    public int AccessFailedCount { get; set; }

    public DateTime? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public string HashedPassword { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public DateTime AccountCreatedDateTime { get; set; }

    public DateTime LastPasswordChangeDateTime { get; set; }

    public DateTime? PasswordChangeDateTime { get; set; }

    public Guid? PasswordChangeGuid { get; set; }

    public virtual ICollection<ApplicationAuthenticationProviderUserMapping> ApplicationAuthenticationProviderUserMappings { get; } = new List<ApplicationAuthenticationProviderUserMapping>();

    public virtual ICollection<ApplicationAuthenticationProviderUserToken> ApplicationAuthenticationProviderUserTokens { get; } = new List<ApplicationAuthenticationProviderUserToken>();

    public virtual ICollection<ApplicationUser> ApplicationUsers { get; } = new List<ApplicationUser>();

    public virtual ICollection<UserPermission> UserPermissions { get; } = new List<UserPermission>();

    public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
}
