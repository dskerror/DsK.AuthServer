using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Security.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationRolePermissionsController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public ApplicationRolePermissionsController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpGet]
        [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationRolesPermissions.View}")]
        public async Task<IActionResult> ApplicationRolePermissionsGet(int applicationId, int applicationRoleId)
        {
            if(applicationRoleId == 0)            
                return BadRequest();

            var result = await SecurityService.ApplicationRolePermissionsGet(applicationId, applicationRoleId);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationRolesPermissions.Edit}")]
        public async Task<IActionResult> ApplicationRolePermissionChange(ApplicationRolePermissionChangeDto model)
        {
            var result = await SecurityService.ApplicationRolePermissionChange(model);
            return Ok(result);
        }
    }
}

