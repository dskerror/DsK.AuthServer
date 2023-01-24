using BlazorWASMCustomAuth.Validations;
using System.ComponentModel.DataAnnotations;

namespace BlazorWASMCustomAuth.Security.Shared
{
    public class UserRoleCreateDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

    }
}
