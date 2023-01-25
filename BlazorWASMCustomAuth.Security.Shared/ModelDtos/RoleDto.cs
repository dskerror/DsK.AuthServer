using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.Shared;

public partial class RoleDto
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public string RoleDescription { get; set; } = null!;

    public virtual ICollection<RolePermissionDto> RolePermissions { get; } = new List<RolePermissionDto>();

    public virtual ICollection<UserRoleDto> UserRoles { get; } = new List<UserRoleDto>();
}
