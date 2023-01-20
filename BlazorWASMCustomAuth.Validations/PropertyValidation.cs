using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMCustomAuth.Validations
{
    public static class PropertyValidation
    {
        public static PropertyValidationResult StringMaxLength(string PropertyName, string PropertyValue, int maxLength)
        {
            var result = new PropertyValidationResult();
            result.PropertyName = PropertyName;
            if (PropertyValue.Length <= maxLength)
            {
                result.IsValid = true;
            }
            else
            {
                result.IsValid = false;
                result.Message = $"{PropertyName} length must be less than {maxLength}";
            }
            return result;
        }
        public static PropertyValidationResult StringMinLength(string PropertyName, string PropertyValue, int minLength)
        {
            var result = new PropertyValidationResult();
            result.PropertyName = PropertyName;
            if (PropertyValue.Length >= minLength)
            {
                result.IsValid = true;
            }
            else
            {
                result.IsValid = false;
                result.Message = $"{PropertyName} length must be greater than {minLength}"; ;
            }
            return result;
        }
        public static PropertyValidationResult IsEmail(string PropertyName, string PropertyValue)
        {
            var result = new PropertyValidationResult();
            result.PropertyName = PropertyName;

            try
            {
                var emailAddress = new MailAddress(PropertyValue);
                result.IsValid = true;
            }
            catch
            {
                result.IsValid = false;
                result.Message = $"{PropertyName} is not a valid email.";
            }
           
            return result;
        }
        public static PropertyValidationResult IsNumber(string PropertyName, string PropertyValue)
        {
            var result = new PropertyValidationResult();
            result.PropertyName = PropertyName;

            if (PropertyValue.All(char.IsDigit))
            {
                result.IsValid = true;
            }
            else
            {
                result.IsValid = false;
                result.Message = $"{PropertyName} isn't a number";
            }

            return result;
        }
        public static PropertyValidationResult IsBool(string PropertyName, string PropertyValue)
        {
            var result = new PropertyValidationResult();
            result.PropertyName = PropertyName;
            Boolean temp;
            if (Boolean.TryParse(PropertyValue, out temp))
            {
                result.IsValid = true;
            }
            else
            {
                result.IsValid = false;
                result.Message = $"{PropertyName} isn't a True/False";
            }

            return result;
        }
        public static PropertyValidationResult IsDate(string PropertyName, string PropertyValue)
        {
            var result = new PropertyValidationResult();
            result.PropertyName = PropertyName;
            DateTime temp;
            if (DateTime.TryParse(PropertyValue, out temp))
            {
                result.IsValid = true;
            }
            else
            {
                result.IsValid = false;
                result.Message = $"{PropertyName} isn't a date";
            }

            return result;
        }
        public static PropertyValidationResult CantContainSpace(string PropertyName, string PropertyValue)
        {
            var result = new PropertyValidationResult();
            result.PropertyName = PropertyName;
            if (PropertyValue.Contains(" "))
            {
                result.IsValid = false;
                result.Message = $"{PropertyName} can't contain spaces";
            }
            else
            {
                result.IsValid = true;
            }

            return result;
        }
        public static PropertyValidationResult StringLenghtBetween(string PropertyName, string PropertyValue, int minLength, int maxLength)
        {
            var result = new PropertyValidationResult();
            result.PropertyName = PropertyName;
            if (PropertyValue.Length > minLength && PropertyValue.Length < maxLength)
            {
                result.IsValid = true;
            }
            else
            {
                result.IsValid = false;
                result.Message = $"{PropertyName} length must be between {minLength} and {maxLength}";
            }
            return result;
        }
        public static PropertyValidationResult Required(string PropertyName, string PropertyValue)
        {
            var result = new PropertyValidationResult();
            result.PropertyName = PropertyName;
            if (PropertyValue.Trim().Length != 0)
            {
                result.IsValid = true;
            }
            else
            {
                result.IsValid = false;
                result.Message = $"{PropertyName} is required and can't be empty.";
            }
            return result;
        }
        public static PropertyValidationResult IntMax(string PropertyName, int PropertyValue, int maxLength)
        {
            var result = new PropertyValidationResult();
            result.PropertyName = PropertyName;
            if (PropertyValue < maxLength)
            {
                result.IsValid = true;
            }
            else
            {
                result.IsValid = false;
                result.Message = $"{PropertyName} can't be greater than {maxLength}";
            }
            return result;
        }
        public static PropertyValidationResult IntMin(string PropertyName, int PropertyValue, int minLength)
        {
            var result = new PropertyValidationResult();
            result.PropertyName = PropertyName;
            if (PropertyValue > minLength)
            {
                result.IsValid = true;
            }
            else
            {
                result.IsValid = false;
                result.Message = $"{PropertyName} can't be greater than {minLength}"; ;
            }
            return result;
        }
        public static PropertyValidationResult IntBetween(string PropertyName, int PropertyValue, int minLength, int maxLength)
        {
            var result = new PropertyValidationResult();
            result.PropertyName = PropertyName;
            if (PropertyValue > minLength && PropertyValue < maxLength)
            {
                result.IsValid = true;
            }
            else
            {
                result.IsValid = false;
                result.Message = $"{PropertyName} value be between {minLength} and {maxLength}";
            }
            return result;
        }
        public static PropertyValidationResult GreaterNumber(string PropertyName, int PropertyValue, int lowerNumber)
        {
            var result = new PropertyValidationResult();
            result.PropertyName = PropertyName;
            if (PropertyValue > lowerNumber)
            {
                result.IsValid = true;
            }
            else
            {
                result.IsValid = false;
                result.Message = $"{PropertyName} must be greater than {lowerNumber}"; ;
            }
            return result;
        }
        public static PropertyValidationResult NotEqualNumber(string PropertyName, int PropertyValue, int otherNumber)
        {
            var result = new PropertyValidationResult();
            result.PropertyName = PropertyName;
            if (PropertyValue != otherNumber)
            {
                result.IsValid = true;
            }
            else
            {
                result.IsValid = false;
                result.Message = $"{PropertyName} can't be {otherNumber}"; ;
            }
            return result;
        }
        public static PropertyValidationResult EqualNumber(string PropertyName, int PropertyValue, int otherNumber)
        {
            var result = new PropertyValidationResult();
            result.PropertyName = PropertyName;
            if (PropertyValue == otherNumber)
            {
                result.IsValid = true;
            }
            else
            {
                result.IsValid = false;
                result.Message = $"{PropertyName} must be be {otherNumber}"; ;
            }
            return result;
        }
    }

    public class PropertyValidationResult
    {
        public PropertyValidationResult()
        {
            PropertyName = string.Empty;
            IsValid = false;
            Message = string.Empty;
        }

        public string PropertyName { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }

}
