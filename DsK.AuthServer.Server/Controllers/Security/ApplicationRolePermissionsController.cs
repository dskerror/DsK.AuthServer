using DsK.AuthServer.Security.Infrastructure;
using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DsK.AuthServer.Server.Controllers.Security;
[Route("[controller]")]
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
    public async Task<IActionResult> ApplicationRolePermissionsGet(int ApplicationId, int ApplicationRoleId)
    {
        if(ApplicationRoleId == 0)            
            return BadRequest();

        var result = await SecurityService.ApplicationRolePermissionsGet(ApplicationId, ApplicationRoleId);
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

