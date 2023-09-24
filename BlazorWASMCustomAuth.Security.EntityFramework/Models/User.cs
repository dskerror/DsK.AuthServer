using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class User
{
    public int Id { get; set; }

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

    public virtual ICollection<ApplicationUser> ApplicationUsers { get; } = new List<ApplicationUser>();

    public virtual ICollection<UserAuthenticationProviderMapping> UserAuthenticationProviderMappings { get; } = new List<UserAuthenticationProviderMapping>();

    public virtual ICollection<UserPermission> UserPermissions { get; } = new List<UserPermission>();

    public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();

    public virtual ICollection<UserToken> UserTokens { get; } = new List<UserToken>();
}
