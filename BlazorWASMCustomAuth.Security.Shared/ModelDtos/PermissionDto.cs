using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.Shared;

public partial class PermissionDto
{
    public int Id { get; set; }

    public string PermissionName { get; set; } = null!;

    public string PermissionDescription { get; set; } = null!;

    public virtual ICollection<RolePermissionDto> RolePermissions { get; } = new List<RolePermissionDto>();

    public virtual ICollection<UserPermissionDto> UserPermissions { get; } = new List<UserPermissionDto>();
}
