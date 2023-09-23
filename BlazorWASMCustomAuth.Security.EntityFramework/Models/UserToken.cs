using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class UserToken
{
    public int Id { get; set; }

    public int? ApplicationId { get; set; }

    public int UserId { get; set; }

    public Guid LoginToken { get; set; }

    public string RefreshToken { get; set; } = null!;

    public DateTime TokenCreatedDateTime { get; set; }

    public DateTime TokenRefreshedDateTime { get; set; }

    public virtual Application? Application { get; set; }

    public virtual User User { get; set; } = null!;
}
