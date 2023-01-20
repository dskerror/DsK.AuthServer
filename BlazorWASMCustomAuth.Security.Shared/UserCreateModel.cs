using BlazorWASMCustomAuth.Validations;
using System.Runtime.CompilerServices;

namespace BlazorWASMCustomAuth.Security.Shared
{
	public class UserCreateModel
    {
		public string Username { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
        public UserCreateModel()
		{
			Username = "";
            FullName = "";
            Email = "";
        }

		public ModelValidation ValidateModel()
		{
            ModelValidation modelValidation = new ModelValidation();

            modelValidation.Validate(PropertyValidation.StringMinLength("Username", Username, 3));
            modelValidation.Validate(PropertyValidation.StringMaxLength("Username", Username, 256));
			modelValidation.Validate(PropertyValidation.Required("Username", Username));
            modelValidation.Validate(PropertyValidation.CantContainSpace("Username", Username));

            modelValidation.Validate(PropertyValidation.StringMinLength("FullName", FullName, 3));
            modelValidation.Validate(PropertyValidation.StringMaxLength("FullName", FullName, 256));
			modelValidation.Validate(PropertyValidation.Required("FullName", FullName));

            modelValidation.Validate(PropertyValidation.StringMinLength("Email", Email, 6));
            modelValidation.Validate(PropertyValidation.StringMaxLength("Email", Email, 256));
            modelValidation.Validate(PropertyValidation.Required("Email", Email));
            modelValidation.Validate(PropertyValidation.IsEmail("Email", Email));

            return modelValidation;
		}
	}
}
