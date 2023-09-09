using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class UserPermission
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PermissionId { get; set; }

    public bool Allow { get; set; }

    public virtual ApplicationPermission Permission { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
