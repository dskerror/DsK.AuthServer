using System;
using System.Collections.Generic;

namespace DsK.AuthServer.Security.EntityFramework.Models;

public partial class ApplicationRole
{
    public int Id { get; set; }

    public bool IsEnabled { get; set; }

    public int ApplicationId { get; set; }

    public string RoleName { get; set; } = null!;

    public string RoleDescription { get; set; } = null!;

    public virtual Application Application { get; set; } = null!;

    public virtual ICollection<ApplicationRolePermission> ApplicationRolePermissions { get; } = new List<ApplicationRolePermission>();

    public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
}
