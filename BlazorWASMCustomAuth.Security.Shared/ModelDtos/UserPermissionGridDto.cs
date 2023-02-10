using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.Shared;

public partial class UserPermissionGridDto
{   
    public int Id { get; set; }
    public string PermissionName { get; set; }
    public string PermissionDescription { get; set; }
    public string Roles { get; set; }
    public bool Enable { get; set; }
}
