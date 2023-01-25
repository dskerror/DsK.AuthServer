using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared
{ 
    public class RoleUpdateDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? RoleName { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string? RoleDescription { get; set; }

    }
}
