using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.Shared;

public partial class ApplicationPermissionDto
{
    public int Id { get; set; }

    public string PermissionName { get; set; } = null!;

    public string PermissionDescription { get; set; } = null!;

    public virtual ICollection<ApplicationRolePermissionDto> RolePermissions { get; } = new List<ApplicationRolePermissionDto>();

    public virtual ICollection<UserPermissionDto> UserPermissions { get; } = new List<UserPermissionDto>();
}
