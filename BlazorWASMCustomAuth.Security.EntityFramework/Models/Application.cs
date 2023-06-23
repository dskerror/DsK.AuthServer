using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class Application
{
    public int Id { get; set; }

    public string ApplicationName { get; set; } = null!;

    public string ApplicationDesc { get; set; } = null!;

    public string AppApiKey { get; set; } = null!;
}
