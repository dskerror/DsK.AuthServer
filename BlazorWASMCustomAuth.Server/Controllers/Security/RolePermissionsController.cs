using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security
{
    [Route("api/Security/[controller]")]
    [ApiController]
    public class RolePermissionsController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public RolePermissionsController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpGet]
        //[Authorize(Roles = "admin,RolePermissionsGet")]        
        public async Task<IActionResult> RolePermissionsGet(int roleid = 0)
        {
            var result = await SecurityService.RolePermissionsGet(roleid);
            return Ok(result);
        }


        [HttpPost]
        //[Authorize(Roles = "admin,RoleCreate")]
        public async Task<IActionResult> RolePermissionCreate(RolePermissionCreateDto model)
        {
            var result = await SecurityService.RolePermissionCreate(model);
            return Ok(result);
        }

        [HttpDelete]
        //[Authorize(Roles = "admin,RoleDelete")]
        public async Task<IActionResult> RolePermissionDelete(RolePermissionDeleteDto model)
        {
            var result = await SecurityService.RolePermissionDelete(model);
            return Ok(result);
        }
    }
}

