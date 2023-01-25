using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.Shared;

public partial class UserPermissionCreateDto
{
    public int UserId { get; set; }

    public int PermissionId { get; set; }

    public bool Allow { get; set; }    
}
