namespace BlazorWASMCustomAuth.Security.Shared;

    public class APIResultNew<T> where T : class
    {
        public APIResultNew()
        {
            HasError = false;
            Message = "";
            Exception = null;            
        }
        public T? Result { get; set; }
        public string Message { get; set; }
        public bool HasError { get; set; }
        public Exception? Exception { get; set; }
    }

