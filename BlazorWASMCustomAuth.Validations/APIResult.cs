using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlazorWASMCustomAuth.Validations
{
    public class APIResult
    {
        public APIResult(object result)
        {
            HasError = false;
            Message = "";
            Exception = null;
            this.Result = result;
            ModelValidationResult = new ModelValidation();
        }

        public APIResult(object result, Exception ex)
        {
            HasError = true;
            Message = "";
            Exception = ex;
            this.Result = result;
            ModelValidationResult = new ModelValidation();
        }
        public object? Result { get; set; }

        public string Message { get; set; }

        public bool HasError { get; set; }

        public Exception? Exception { get; set; }
        public ModelValidation ModelValidationResult { get; set; }
    }
}
