using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared
{
    public class UserUpdateDto
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string? Name { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 6)]
        [EmailAddress]
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}
