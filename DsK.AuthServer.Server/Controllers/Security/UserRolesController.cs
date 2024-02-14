using DsK.AuthServer.Security.Infrastructure;
using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DsK.AuthServer.Server.Controllers.Security;
[Route("[controller]")]
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

