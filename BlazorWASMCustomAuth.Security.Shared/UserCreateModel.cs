using BlazorWASMCustomAuth.Validations;

namespace BlazorWASMCustomAuth.Security.Shared
{
	public class UserCreateModel
	{
		public string Username { get; set; }
		public string Name { get; set; }
		public string? Email { get; set; }		

		public ModelValidation ValidateModel()
		{
			var mv = new ModelValidation();


            mv.Validate(PropertyValidation.StringMaxLength("Username", Username, 256));
            mv.Validate(PropertyValidation.Required("Username", Username));

            mv.Validate(PropertyValidation.StringMaxLength("Name", Name, 256));
            mv.Validate(PropertyValidation.Required("Name", Name));

            return mv;
        }
	}
}
