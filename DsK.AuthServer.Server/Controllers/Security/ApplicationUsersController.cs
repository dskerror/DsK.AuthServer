using DsK.AuthServer.Security.Infrastructure;
using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DsK.AuthServer.Server.Controllers.Security;
[Route("[controller]")]
[ApiController]
public class ApplicationUsersController : ControllerBase
{
    private readonly SecurityService SecurityService;

    public ApplicationUsersController(SecurityService securityService)
    {
        SecurityService = securityService;
    }

    [HttpGet]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationUsers.View}")]
    public async Task<IActionResult> Get(int applicationId)
    {
        var result = await SecurityService.ApplicationUsersGet(applicationId);
        return Ok(result);
    }
}