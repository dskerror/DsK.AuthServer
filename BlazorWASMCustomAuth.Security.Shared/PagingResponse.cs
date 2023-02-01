namespace BlazorWASMCustomAuth.Security.Shared;
public class PagingResponse
{    
    public int CurrentPage { get; set; } = 1;
    public int TotalItems { get; set; } = 10;
}
