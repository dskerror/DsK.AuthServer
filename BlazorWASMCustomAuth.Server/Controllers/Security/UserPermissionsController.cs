using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security
{
    [Route("api/Security/[controller]")]
    [ApiController]
    public class UserPermissionsController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public UserPermissionsController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpGet]
        //[Authorize(Roles = "admin,RolesGet")]
        public async Task<IActionResult> UserPermissionsGet(int userId)
        {
            if (userId == 0)
                return BadRequest();

            var result = await SecurityService.GetUserPermissions(userId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UserPermissionChange(UserPermissionChangeDto model)
        {
            var result = await SecurityService.UserPermissionChange(model);
            return Ok(result);
        }
    }
}

