using System;
using System.Collections.Generic;

namespace BlazorWASMCustomAuth.Security.Shared;

public partial class UserAuthenticationProviderMappingsGridDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int AuthenticationProviderId { get; set; }

    public string AuthenticationProviderName { get; set; } = null!;

    public string AuthenticationProviderType { get; set; } = null!;

    public string Username { get; set; } = null!;

}
