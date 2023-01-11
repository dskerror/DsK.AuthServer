using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class UserLog
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime EventDateTime { get; set; }

    public string Event { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
