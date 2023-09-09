using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Security.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public PermissionsController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpGet]
        [Authorize(Roles = $"{Access.Admin}, {Access.Permissions.View}")]
        public async Task<IActionResult> PermissionsGet(int id = 0)
        {
            var result = await SecurityService.PermissionsGet(id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = $"{Access.Admin}, {Access.Permissions.Create}")]
        public async Task<IActionResult> PermissionCreate(PermissionCreateDto model)
        {
            var result = await SecurityService.PermissionCreate(model);
            return Ok(result);
        }
        [HttpPut]
        [Authorize(Roles = $"{Access.Admin}, {Access.Permissions.Edit}")]
        public async Task<IActionResult> PermissionUpdate(PermissionUpdateDto model)
        {
            var result = await SecurityService.PermissionUpdate(model);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = $"{Access.Admin}, {Access.Permissions.Delete}")]
        public async Task<IActionResult> PermissionDelete(int id)
        {
            var result = await SecurityService.PermissionDelete(id);
            return Ok(result);
        }
    }
}

