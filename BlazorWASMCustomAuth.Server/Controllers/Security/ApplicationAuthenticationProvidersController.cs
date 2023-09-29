using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security;

[Route("api/[controller]")]
[ApiController]
public class ApplicationAuthenticationProvidersController : ControllerBase
{
    private readonly SecurityService SecurityService;
    public ApplicationAuthenticationProvidersController(SecurityService securityService)
    {
        SecurityService = securityService;
    }

    [HttpPost]        
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationAuthenticationProvider.Create}")]
    public async Task<IActionResult> ApplicationAuthenticationProvidersCreate(ApplicationAuthenticationProviderCreateDto model)
    {
        var result = await SecurityService.ApplicationAuthenticationProvidersCreate(model);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationAuthenticationProvider.View}")]
    public async Task<IActionResult> ApplicationAuthenticationProvidersGet(int applicationId, int id, int pageNumber, int pageSize, string searchString = null, string orderBy = null)
    {
        var result = await SecurityService.ApplicationAuthenticationProvidersGet(applicationId, id, pageNumber, pageSize, searchString, orderBy);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationAuthenticationProvider.Edit}")]
    public async Task<IActionResult> ApplicationAuthenticationProvidersUpdate(ApplicationAuthenticationProviderDto model)
    {
        var result = await SecurityService.ApplicationAuthenticationProvidersUpdate(model);
        return Ok(result);
    }

    [HttpDelete]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationAuthenticationProvider.Delete}")]
    public async Task<IActionResult> ApplicationAuthenticationProvidersDelete(int id)
    {
        var result = await SecurityService.ApplicationAuthenticationProvidersDelete(id);
        return Ok(result);
    }

    [HttpPost("DisableEnable")]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationAuthenticationProvider.DisableEnable}")]
    public async Task<IActionResult> DisableEnable([FromBody] int id)
    {
        var result = await SecurityService.ApplicationAuthenticationProviderDisableEnabled(id);
        return Ok(result);
    }
}

