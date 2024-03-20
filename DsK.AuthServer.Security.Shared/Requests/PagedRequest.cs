namespace DsK.AuthServer.Security.Shared;
public class PagedRequest
{
    
    private string? _orderBy = null;

    public PagedRequest()
    {
        _orderBy = "Id";
    }

    public int Id { get; set; } = 0;
    public string? SearchString { get; set; } = null;
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public string? OrderBy
    {
        get { return _orderBy; }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                string[] OrderBy = value.Split(',');
                _orderBy = string.Join(",", OrderBy);
            }
            else 
                _orderBy = "Id";
        }
    }
}
