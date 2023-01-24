using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security
{
    [Route("api/Security/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public UserRolesController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpPost]
        //[Authorize(Roles = "admin,RoleCreate")]
        public IActionResult UserRoleCreate(UserRoleCreateDto model)
        {
            var result = SecurityService.UserRoleCreate(model);
            return Ok(result);
        }

        [HttpGet]
        //[Authorize(Roles = "admin,RolesGet")]
        public IActionResult UserRolesGet(int userId = 0)
        {
            return Ok(SecurityService.UserRolesGet(userId));
        }

        [HttpDelete]
        //[Authorize(Roles = "admin,RoleDelete")]
        public IActionResult UserRoleDelete(int id)
        {
            var result = SecurityService.UserRoleDelete(id);
            return Ok(result);
        }
    }
}

