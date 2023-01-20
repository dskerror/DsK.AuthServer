using BlazorWASMCustomAuth.Validations;
using System;

namespace BlazorWASMCustomAuth.Security.Shared
{
    public class UserUpdateModel
    {
        public int Id { get; set; } 
		public string? Name { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public bool TwoFactorEnabled { get; set; }

        public ModelValidation ValidateModel()
        {
            ModelValidation modelValidation = new ModelValidation();

            modelValidation.Validate(PropertyValidation.StringMinLength("Name", Name, 3));
            modelValidation.Validate(PropertyValidation.StringMaxLength("Name", Name, 256));
            modelValidation.Validate(PropertyValidation.Required("Name", Name));

            modelValidation.Validate(PropertyValidation.StringMinLength("Email", Email, 6));
            modelValidation.Validate(PropertyValidation.StringMaxLength("Email", Email, 256));
            modelValidation.Validate(PropertyValidation.Required("Email", Email));
            modelValidation.Validate(PropertyValidation.IsEmail("Email", Email));

            return modelValidation;
        }
    }
}
