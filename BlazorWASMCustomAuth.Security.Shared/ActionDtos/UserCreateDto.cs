using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace BlazorWASMCustomAuth.Security.Shared
{
	public class UserCreateDto
    {
        [Required]
        [StringLength(256, MinimumLength = 3)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Username can't contain spaces.")]
        [DefaultValue("jsmith")]
        public string? Username { get; set; }
         
        [Required]
        [StringLength(256, MinimumLength = 3)]
        [DefaultValue("John Smith")]
        public string? Name { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 6)]
        [EmailAddress]
        [DefaultValue("jsmith@gmail.com")]
        public string? Email { get; set; }
	}
}
