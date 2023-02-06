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

        [HttpPost]
        //[Authorize(Roles = "admin,RoleCreate")]
        public async Task<IActionResult> UserPermissionCreate(UserPermissionCreateDto model)
        {
            var result = await SecurityService.UserPermissionCreate(model);
            return Ok(result);
        }

        [HttpGet]
        //[Authorize(Roles = "admin,RolesGet")]
        public async Task<IActionResult> UserPermissionGet(string username)
        {
            var result = await SecurityService.GetUserPermissions(username);
            return Ok(result);
        }        

        [HttpDelete]
        //[Authorize(Roles = "admin,RoleDelete")]
        public async Task<IActionResult> UserPermissionDelete(int id)
        {
            var result = await SecurityService.UserPermissionDelete(id);
            return Ok(result);
        }
    }
}

