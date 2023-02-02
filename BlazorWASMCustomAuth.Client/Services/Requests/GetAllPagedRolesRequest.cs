namespace BlazorWASMCustomAuth.Client.Services.Requests
{
    public class GetAllPagedRolesRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
