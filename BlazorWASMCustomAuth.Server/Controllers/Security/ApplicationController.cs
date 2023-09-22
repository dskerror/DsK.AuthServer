using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security;

[Route("api/[controller]")]
[ApiController]
public class ApplicationController : ControllerBase
{
    private readonly SecurityService SecurityService;
    public ApplicationController(SecurityService securityService)
    {
        SecurityService = securityService;
    }

    [HttpPost]        
    [Authorize(Roles = $"{Access.Admin}, {Access.Application.Create}")]
    public async Task<IActionResult> ApplicationCreate(ApplicationCreateDto model)
    {
        var result = await SecurityService.ApplicationCreate(model);
        return Ok(result);
    }

    [HttpPost("GenerateNewAPIKey")]        
    [Authorize(Roles = $"{Access.Admin}, {Access.Application.GenerateNewAPIKey}")]
    public async Task<IActionResult> GenerateNewAPIKey(ApplicationDto model)
    {
        var result = await SecurityService.ApplicationGenerateNewAPIKey(model.Id);
        return Ok(result);
    }

    [HttpPost("DisableEnable")]
    [Authorize(Roles = $"{Access.Admin}, {Access.Application.DisableEnable}")]
    public async Task<IActionResult> DisableEnable([FromBody] int id)
    {
        var result = await SecurityService.ApplicationDisableEnabled(id);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = $"{Access.Admin}, {Access.Application.View}")]
    public async Task<IActionResult> ApplicationGet(int id, int pageNumber, int pageSize, string searchString = null, string orderBy = null)
    {
        var result = await SecurityService.ApplicationGet(id, pageNumber, pageSize, searchString, orderBy);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{Access.Admin}, {Access.Application.Edit}")]
    public async Task<IActionResult> ApplicationUpdate(ApplicationUpdateDto model)
    {
        var result = await SecurityService.ApplicationUpdate(model);
        return Ok(result);
    }

    [HttpDelete]
    [Authorize(Roles = $"{Access.Admin}, {Access.Application.Delete}")]
    public async Task<IActionResult> ApplicationDelete(int id)
    {
        var result = await SecurityService.ApplicationDelete(id);
        return Ok(result);
    }
}

