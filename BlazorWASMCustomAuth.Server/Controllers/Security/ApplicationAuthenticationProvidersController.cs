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
    [Authorize(Roles = $"{Access.Admin}, {Access.AuthenticationProvider.Create}")]
    public async Task<IActionResult> ApplicationAuthenticationProvidersCreate(ApplicationAuthenticationProviderCreateDto model)
    {
        var result = await SecurityService.ApplicationAuthenticationProvidersCreate(model);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = $"{Access.Admin}, {Access.AuthenticationProvider.View}")]
    public async Task<IActionResult> ApplicationAuthenticationProvidersGet(int applicationId, int id, int pageNumber, int pageSize, string searchString = null, string orderBy = null)
    {
        var result = await SecurityService.ApplicationAuthenticationProvidersGet(applicationId, id, pageNumber, pageSize, searchString, orderBy);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{Access.Admin}, {Access.AuthenticationProvider.Edit}")]
    public async Task<IActionResult> ApplicationAuthenticationProvidersUpdate(ApplicationAuthenticationProviderUpdateDto model)
    {
        var result = await SecurityService.ApplicationAuthenticationProvidersUpdate(model);
        return Ok(result);
    }

    [HttpDelete]
    [Authorize(Roles = $"{Access.Admin}, {Access.AuthenticationProvider.Delete}")]
    public async Task<IActionResult> ApplicationAuthenticationProvidersDelete(int id)
    {
        var result = await SecurityService.ApplicationAuthenticationProvidersDelete(id);
        return Ok(result);
    }
}

