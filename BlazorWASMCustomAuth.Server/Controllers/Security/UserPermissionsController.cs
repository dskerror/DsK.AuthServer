using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security;
[Route("api/[controller]")]
[ApiController]
public class UserPermissionsController : ControllerBase
{
    private readonly SecurityService SecurityService;
    public UserPermissionsController(SecurityService securityService)
    {
        SecurityService = securityService;
    }

    [HttpGet]
    [Authorize(Roles = $"{Access.Admin}, {Access.UserPermissions.View}")]
    public async Task<IActionResult> UserPermissionsGet(int userId)
    {
        if (userId == 0)
            return BadRequest();

        var result = await SecurityService.GetUserPermissions(userId);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = $"{Access.Admin}, {Access.UserPermissions.Edit}")]
    public async Task<IActionResult> UserPermissionChange(UserPermissionChangeDto model)
    {
        var result = await SecurityService.UserPermissionChange(model);
        return Ok(result);
    }
}

