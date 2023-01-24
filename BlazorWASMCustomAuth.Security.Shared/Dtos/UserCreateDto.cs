using BlazorWASMCustomAuth.Validations;
using System.ComponentModel.DataAnnotations;


namespace BlazorWASMCustomAuth.Security.Shared
{
	public class UserCreateDto
    {
        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string? Username { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string? FullName { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 6)]
        [EmailAddress]
        public string? Email { get; set; }

        public ModelValidation ValidateModel()
		{
            ModelValidation modelValidation = new ModelValidation();
            modelValidation.Validate(PropertyValidation.CantContainSpace("Username", Username));
            return modelValidation;
		}
	}
}
