using System;
using System.Collections.Generic;

namespace DsK.AuthServer.Security.EntityFramework.Models;

public partial class UserRole
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int RoleId { get; set; }

    public virtual ApplicationRole Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
