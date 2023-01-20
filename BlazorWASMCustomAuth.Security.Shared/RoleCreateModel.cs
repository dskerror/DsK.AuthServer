using BlazorWASMCustomAuth.Validations;

namespace BlazorWASMCustomAuth.Security.Shared
{
    public class RoleCreateModel
    {   
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }

        public ModelValidation ValidateModel()
        {
            ModelValidation modelValidation = new ModelValidation();

            modelValidation.Validate(PropertyValidation.StringMinLength("RoleName", RoleName, 3));
            modelValidation.Validate(PropertyValidation.StringMaxLength("RoleName", RoleName, 50));
            modelValidation.Validate(PropertyValidation.Required("RoleName", RoleName));

            modelValidation.Validate(PropertyValidation.StringMinLength("RoleDescription", RoleDescription, 3));
            modelValidation.Validate(PropertyValidation.StringMaxLength("RoleDescription", RoleDescription, 250));
            modelValidation.Validate(PropertyValidation.Required("RoleDescription", RoleDescription));

            return modelValidation;
        }
    }
}
