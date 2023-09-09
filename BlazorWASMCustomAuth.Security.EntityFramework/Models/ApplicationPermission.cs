using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class ApplicationPermission
{
    public int Id { get; set; }

    public string PermissionName { get; set; } = null!;

    public string PermissionDescription { get; set; } = null!;

    public virtual ICollection<ApplicationRolePermission> ApplicationRolePermissions { get; } = new List<ApplicationRolePermission>();

    public virtual ICollection<UserPermission> UserPermissions { get; } = new List<UserPermission>();
}
