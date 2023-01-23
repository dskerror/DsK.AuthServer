using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class Permission
{
    public int Id { get; set; }

    public string PermissionName { get; set; } = null!;

    public string PermissionDescription { get; set; } = null!;

    public virtual ICollection<RolePermission> RolePermissions { get; } = new List<RolePermission>();

    public virtual ICollection<UserPermission> UserPermissions { get; } = new List<UserPermission>();
}
