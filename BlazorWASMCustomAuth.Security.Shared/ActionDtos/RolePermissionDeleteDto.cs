using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.Shared;

public partial class RolePermissionDeleteDto
{
    public int RoleId { get; set; }

    public int PermissionId { get; set; }
}
