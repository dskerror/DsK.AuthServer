using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class ApplicationRolePermission
{
    public int RoleId { get; set; }

    public int PermissionId { get; set; }

    public virtual ApplicationPermission Role { get; set; } = null!;

    public virtual ApplicationRole RoleNavigation { get; set; } = null!;
}
