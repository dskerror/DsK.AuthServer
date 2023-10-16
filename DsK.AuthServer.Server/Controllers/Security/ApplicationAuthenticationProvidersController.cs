using DsK.AuthServer.Security.Infrastructure;
using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DsK.AuthServer.Server.Controllers.Security;

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
    public async Task<IActionResult> Create(ApplicationAuthenticationProviderCreateDto model)
    {
        var result = await SecurityService.ApplicationAuthenticationProvidersCreate(model);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationAuthenticationProvider.View}")]
    public async Task<IActionResult> Get(int applicationId, int id, int pageNumber, int pageSize, string searchString = null, string orderBy = null)
    {
        var result = await SecurityService.ApplicationAuthenticationProvidersGet(applicationId, id, pageNumber, pageSize, searchString, orderBy);
        return Ok(result);
    }

    [HttpGet]
    [Route("ValidateApplicationAuthenticationProviderGuid")]
    public async Task<IActionResult> ValidateApplicationAuthenticationProviderGuid(string ApplicationAuthenticationProviderGuid)
    {
        var result = await SecurityService.ValidateApplicationAuthenticationProviderGuid(ApplicationAuthenticationProviderGuid);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationAuthenticationProvider.Edit}")]
    public async Task<IActionResult> Update(ApplicationAuthenticationProviderDto model)
    {
        var result = await SecurityService.ApplicationAuthenticationProvidersUpdate(model);
        return Ok(result);
    }

    [HttpDelete]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationAuthenticationProvider.Delete}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await SecurityService.ApplicationAuthenticationProvidersDelete(id);
        return Ok(result);
    }

    [HttpPost("IsEnabledToggle")]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationAuthenticationProvider.IsEnabledToggle}")]
    public async Task<IActionResult> IsEnabledToggle([FromBody] int id)
    {
        var result = await SecurityService.ApplicationAuthenticationProviderIsEnabledToggle(id);
        return Ok(result);
    }


    [HttpPost("ValidateDomainConnection")]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationAuthenticationProvider.Edit}")]    
    public async Task<IActionResult> ValidateDomainConnection(ApplicationAuthenticationProviderValidateDomainConnectionDto model)
    {
        var IsValid = await SecurityService.ValidateDomainConnection(model);
        return Ok(IsValid);
    }
}

