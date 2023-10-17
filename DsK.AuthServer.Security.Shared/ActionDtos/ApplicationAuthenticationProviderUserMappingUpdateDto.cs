using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DsK.AuthServer.Security.Shared;
public class ApplicationAuthenticationProviderUserMappingUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]    
    public string? Username { get; set; }
}
