using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security
{
    [Route("api/Security/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public PermissionsController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpGet]
        //[Authorize(Roles = "admin,PermissionsGet")]
        public async Task<IActionResult> PermissionsGet(int id = 0)
        {
            var result = await SecurityService.PermissionsGet(id);
            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Roles = "admin,PermissionCreate")]
        public async Task<IActionResult> PermissionCreate(PermissionCreateDto model)
        {
            var result = await SecurityService.PermissionCreate(model);
            return Ok(result);
        }
        [HttpPut]
        //[Authorize(Roles = "admin,PermissionUpdate")]
        public async Task<IActionResult> PermissionUpdate(PermissionUpdateDto model)
        {
            var result = await SecurityService.PermissionUpdate(model);
            return Ok(result);
        }

        [HttpDelete]
        //[Authorize(Roles = "admin,PermissionDelete")]
        public async Task<IActionResult> PermissionDelete(int id)
        {
            var result = await SecurityService.PermissionDelete(id);
            return Ok(result);
        }
    }
}

