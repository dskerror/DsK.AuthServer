
namespace BlazorWASMCustomAuth.Security.Shared
{
    public class RolePermissionModel
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }
        public string? PermissionDescription { get; set; }
    }
}
