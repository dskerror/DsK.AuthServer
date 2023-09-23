using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security;
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
    public async Task<IActionResult> ApplicationRolePermissionsGet(ApplicationRolePermissionsGetDto model)
    {
        if(model.ApplicationRoleId == 0)            
            return BadRequest();

        var result = await SecurityService.ApplicationRolePermissionsGet(model);
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

