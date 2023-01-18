using BlazorWASMCustomAuth.Validations;

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
			var mv = new ModelValidation();

            mv.Validate(PropertyValidation.StringMinLength("Username", Username, 3));
            mv.Validate(PropertyValidation.StringMaxLength("Username", Username, 256));
			mv.Validate(PropertyValidation.Required("Username", Username));
            mv.Validate(PropertyValidation.CantContainSpace("Username", Username));


            mv.Validate(PropertyValidation.StringMinLength("FullName", FullName, 3));
            mv.Validate(PropertyValidation.StringMaxLength("FullName", FullName, 256));
			mv.Validate(PropertyValidation.Required("FullName", FullName));

            mv.Validate(PropertyValidation.StringMinLength("Email", Email, 6));
            mv.Validate(PropertyValidation.StringMaxLength("Email", Email, 256));
            mv.Validate(PropertyValidation.Required("Email", Email));

            return mv;
		}
	}
}
