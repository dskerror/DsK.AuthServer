using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared
{
    public class PermissionCreateDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? PermissionName { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string? PermissionDescription { get; set; }

    }
}
