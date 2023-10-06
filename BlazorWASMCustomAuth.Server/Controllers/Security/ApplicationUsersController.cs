using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security;
[Route("api/[controller]")]
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