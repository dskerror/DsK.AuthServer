using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.Shared;

public partial class ApplicationRolePermissionDto
{
    public int RoleId { get; set; }

    public int PermissionId { get; set; }

    public virtual ApplicationPermissionDto Role { get; set; } = null!;

    public virtual ApplicationRoleDto RoleNavigation { get; set; } = null!;
}
