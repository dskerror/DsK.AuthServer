using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class ApplicationRolePermission
{
    public int ApplicationRoleId { get; set; }

    public int ApplicationPermissionId { get; set; }

    public virtual ApplicationPermission ApplicationRole { get; set; } = null!;

    public virtual ApplicationRole ApplicationRoleNavigation { get; set; } = null!;
}
