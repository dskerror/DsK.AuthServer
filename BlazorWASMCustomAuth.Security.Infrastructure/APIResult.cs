namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public class APIResult
    {
        public APIResult(object request)
        {
            HasError = false;
            Message = "";
            Exception = null;
            Request = request;
        }
        public object? Request { get; set; }
        public object? Result { get; set; }
        public string Message { get; set; }
        public bool HasError { get; set; }
        public Exception? Exception { get; set; }
    }
}
