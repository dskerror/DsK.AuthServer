using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security;

[Route("api/[controller]")]
[ApiController]
public class UserApplicationsController : ControllerBase
{
    private readonly SecurityService SecurityService;
    public UserApplicationsController(SecurityService securityService)
    {
        SecurityService = securityService;
    }

    [HttpGet]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationUsers.View}")]
    public async Task<IActionResult> Get(int id, int pageNumber, int pageSize, string searchString = null, string orderBy = null)
    {
        var result = await SecurityService.UserApplicationsGet(id, pageNumber, pageSize, searchString, orderBy);
        return Ok(result);
    }
}

