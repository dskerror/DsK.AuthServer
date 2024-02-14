using DsK.AuthServer.Security.Infrastructure;
using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DsK.AuthServer.Server.Controllers.Security;

[Route("[controller]")]
[ApiController]
public class UserApplicationsController : ControllerBase
{
    private readonly SecurityService SecurityService;
    public UserApplicationsController(SecurityService securityService)
    {
        SecurityService = securityService;
    }

    [HttpGet]
    [Authorize(Roles = $"{Access.Admin}, {Access.UserApplications.View}")]
    public async Task<IActionResult> Get(int userId = 0)
    {
        var result = await SecurityService.UserApplicationsGet(userId);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = $"{Access.Admin}, {Access.UserApplications.Edit}, {Access.ApplicationUsers.Edit}")]
    public async Task<IActionResult> UserApplicationChange(ApplicationUserChangeDto model)
    {
        var result = await SecurityService.UserApplicationChange(model);
        return Ok(result);
    }
}

