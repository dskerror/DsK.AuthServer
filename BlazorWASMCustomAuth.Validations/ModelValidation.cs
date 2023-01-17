using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMCustomAuth.Validations
{
    public class ModelValidation
    {
        public ModelValidation()
        {
            ValidationMessages = new List<ModelValidationMessage>();
            IsValid = true;
        }
        public bool IsValid { get; set; }
        public List<ModelValidationMessage> ValidationMessages { get; private set; }

        public void AddMessage(string propertyName, string message) 
        {
            ModelValidationMessage mve = new ModelValidationMessage(propertyName, message);
            ValidationMessages.Add(mve);
        }

        public void Validate(PropertyValidationResult propertyValidationResult)
        {
            if (!propertyValidationResult.IsValid)
            {
                IsValid = false;
                AddMessage(propertyValidationResult.PropertyName, propertyValidationResult.Message);
            }
        }
    }

    public class ModelValidationMessage
    {
        public ModelValidationMessage(string propertyName, string message)
        {
            PropertyName = propertyName;
            Message = message;
        }

        public string PropertyName { get; set; }
        public string Message { get; set; }
    }
}
