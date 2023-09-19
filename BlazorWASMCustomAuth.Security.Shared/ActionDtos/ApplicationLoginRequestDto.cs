using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.Shared;

public partial class ApplicationLoginRequestDto
{   
    public string ApplicationGuid { get; set; }
 
    public string AppApiKey { get; set; }     
}
