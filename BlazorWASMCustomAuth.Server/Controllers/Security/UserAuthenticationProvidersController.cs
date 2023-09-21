using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security;
[Route("api/[controller]")]
[ApiController]
public class UserAuthenticationProvidersController : ControllerBase
{
    private readonly SecurityService SecurityService;
    public UserAuthenticationProvidersController(SecurityService securityService)
    {
        SecurityService = securityService;
    }

    [HttpPost]
    [Authorize(Roles = $"{Access.Admin}, {Access.UserAuthenticationProviders.Create}")]
    public async Task<IActionResult> UserAuthenticationProviderCreate(UserAuthenticationProviderCreateDto model)
    {
        var result = await SecurityService.UserAuthenticationProviderCreate(model);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = $"{Access.Admin}, {Access.UserAuthenticationProviders.View}")]
    public async Task<IActionResult> UserAuthenticationProvidersGet(int userId)
    {
        var result = await SecurityService.UserAuthenticationProvidersGet(userId);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{Access.Admin}, {Access.UserAuthenticationProviders.Edit}")]
    public async Task<IActionResult> UserAuthenticationProviderUpdate(UserAuthenticationProviderUpdateDto model)
    {
        var result = await SecurityService.UserAuthenticationProviderUpdate(model);
        return Ok(result);
    }

    [HttpDelete]
    [Authorize(Roles = $"{Access.Admin}, {Access.UserAuthenticationProviders.Delete}")]
    public async Task<IActionResult> UserAuthenticationProviderDelete(int id)
    {
        var result = await SecurityService.UserAuthenticationProviderDelete(id);
        return Ok(result);
    }
}

