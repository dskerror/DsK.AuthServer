﻿using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.Shared;

public partial class ApplicationDto
{
    public int Id { get; set; }

    public string ApplicationName { get; set; } = null!;

    public string ApplicationDesc { get; set; } = null!;

    public string AppApiKey { get; set; } = null!;
}