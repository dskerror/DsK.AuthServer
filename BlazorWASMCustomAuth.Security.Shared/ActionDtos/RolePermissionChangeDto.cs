using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.Shared;

public partial class RolePermissionChangeDto
{    
    public int RoleId { get; set; }

    public int PermissionId { get; set; }

    public bool PermissionEnabled{ get; set; }
}
