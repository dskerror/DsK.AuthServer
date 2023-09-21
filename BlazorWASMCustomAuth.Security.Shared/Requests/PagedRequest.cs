namespace BlazorWASMCustomAuth.Security.Shared;
public class PagedRequest
{
    public int Id { get; set; } = 0;
    public string SearchString { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public string Orderby { get; set; }
}
