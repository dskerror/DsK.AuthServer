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
        public IActionResult UserPermissionCreate(UserPermission model)
        {
            var result = SecurityService.UserPermissionCreate(model);
            return Ok(result);
        }

        [HttpGet]
        //[Authorize(Roles = "admin,RolesGet")]
        public IActionResult UserPermissionGet(string username)
        {
            return Ok(SecurityService.GetUserPermissions(username));
        }

        [HttpGet]
        [Route("GetUserPermissionsNew")]
        //[Authorize(Roles = "admin,RolesGet")]
        public IActionResult UserPermissionGetNew(string username)
        {
            return Ok(SecurityService.GetUserPermissionsNew(username));
        }

        

        [HttpDelete]
        //[Authorize(Roles = "admin,RoleDelete")]
        public IActionResult UserPermissionDelete(int id)
        {
            var result = SecurityService.UserPermissionDelete(id);
            return Ok(result);
        }
    }
}

