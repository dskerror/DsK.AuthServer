namespace BlazorWASMCustomAuth.Security.Shared;
public partial class ApplicationPermissionGridDto
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public string PermissionName { get; set; }
    public string PermissionDescription { get; set; }
}
