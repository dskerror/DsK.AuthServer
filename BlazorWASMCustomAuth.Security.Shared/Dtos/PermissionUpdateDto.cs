using BlazorWASMCustomAuth.Validations;
using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared
{
    public class PermissionUpdateDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? PermissionName { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string? PermissionDescription { get; set; }

    }
}
