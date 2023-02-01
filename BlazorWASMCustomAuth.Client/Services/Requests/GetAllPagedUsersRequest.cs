namespace BlazorWASMCustomAuth.Client.Services.Requests
{
    public class GetAllPagedUsersRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
