using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class Role
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public string RoleDescription { get; set; } = null!;

    public virtual ICollection<RolePermission> RolePermissions { get; } = new List<RolePermission>();

    public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
}
