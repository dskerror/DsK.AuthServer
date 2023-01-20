using BlazorWASMCustomAuth.Validations;

namespace BlazorWASMCustomAuth.Security.Shared
{
    public class PermissionModel
    {
        public int Id { get; set; }
        public string? PermissionName { get; set; }
        public string? PermissionDescription { get; set; }

        public ModelValidation ValidateModel()
        {
            ModelValidation modelValidation = new ModelValidation();

            modelValidation.Validate(PropertyValidation.StringMinLength("PermissionName", PermissionName, 3));
            modelValidation.Validate(PropertyValidation.StringMaxLength("PermissionName", PermissionName, 50));
            modelValidation.Validate(PropertyValidation.Required("PermissionName", PermissionName));

            modelValidation.Validate(PropertyValidation.StringMinLength("PermissionDescription", PermissionDescription, 3));
            modelValidation.Validate(PropertyValidation.StringMaxLength("PermissionDescription", PermissionDescription, 250));
            modelValidation.Validate(PropertyValidation.Required("PermissionDescription", PermissionDescription));

            return modelValidation;
        }
    }
}
