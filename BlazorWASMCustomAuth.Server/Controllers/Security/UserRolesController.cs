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

        [HttpGet]
        //[Authorize(Roles = "admin,RolesGet")]
        public async Task<IActionResult> UserRolesGet(int userId = 0)
        {
            if (userId == 0)
                return BadRequest();

            var result = await SecurityService.UserRolesGet(userId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UserRoleChange(UserRoleChangeDto model)
        {
            var result = await SecurityService.UserRoleChange(model);
            return Ok(result);
        }


        //[HttpDelete]
        ////[Authorize(Roles = "admin,RoleDelete")]
        //public async Task<IActionResult> UserRoleDelete(int id)
        //{
        //    var result = await SecurityService.UserRoleDelete(id);
        //    return Ok(result);
        //}

        //[HttpPost]
        ////[Authorize(Roles = "admin,RoleCreate")]
        //public async Task<IActionResult> UserRoleCreate(UserRoleCreateDto model)
        //{
        //    var result = await SecurityService.UserRoleCreate(model);
        //    return Ok(result);
        //}
    }
}

