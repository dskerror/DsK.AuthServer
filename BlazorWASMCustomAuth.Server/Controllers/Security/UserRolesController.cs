using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security;
[Route("api/[controller]")]
[ApiController]
public class UserRolesController : ControllerBase
{
    private readonly SecurityService SecurityService;
    public UserRolesController(SecurityService securityService)
    {
        SecurityService = securityService;
    }

    [HttpGet]
    [Authorize(Roles = $"{Access.Admin}, {Access.UserRoles.View}")]
    public async Task<IActionResult> UserRolesGet(int userId = 0)
    {
        if (userId == 0)
            return BadRequest();

        var result = await SecurityService.UserRolesGet(userId);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = $"{Access.Admin}, {Access.UserRoles.Edit}")]
    public async Task<IActionResult> UserRoleChange(UserRoleChangeDto model)
    {
        var result = await SecurityService.UserRoleChange(model);
        return Ok(result);
    }
}

