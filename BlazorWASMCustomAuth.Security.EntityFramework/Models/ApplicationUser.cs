using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class ApplicationUser
{
    public int Id { get; set; }

    public int ApplicationId { get; set; }

    public int UserId { get; set; }
}
