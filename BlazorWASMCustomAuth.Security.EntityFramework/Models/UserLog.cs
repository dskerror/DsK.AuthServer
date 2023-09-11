using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class UserLog
{
    public int Id { get; set; }

    public int ApplicationId { get; set; }

    public int UserId { get; set; }

    public string Ip { get; set; } = null!;

    public string Method { get; set; } = null!;

    public string Path { get; set; } = null!;

    public string QueryString { get; set; } = null!;

    public DateTime LogDateTime { get; set; }

    public virtual Application Application { get; set; } = null!;
}
