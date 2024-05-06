namespace DsK.AuthServer.Security.Shared;

public class APIResponse<T> where T : class
{
    public APIResponse()
    {
        HasError = false;
        Message = "";
        Exception = null;
        Paging = new PagingResponse();
    }
    public T? Result { get; set; }
    public string Message { get; set; }
    public bool HasError { get; set; }
    public PagingResponse Paging { get; set; }
    public Exception? Exception { get; set; }
}

