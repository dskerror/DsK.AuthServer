using DsK.AuthServer.Security.Infrastructure;
using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DsK.AuthServer.Server.Controllers.Security;

[Route("[controller]")]
[ApiController]
public class ApplicationRolesController : ControllerBase
{
    private readonly SecurityService SecurityService;
    public ApplicationRolesController(SecurityService securityService)
    {
        SecurityService = securityService;
    }

    [HttpPost]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationRoles.Create}")]
    public async Task<IActionResult> Create(ApplicationRoleCreateDto model)
    {
        var result = await SecurityService.ApplicationRoleCreate(model);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationRoles.View}")]
    public async Task<IActionResult> Get(int ApplicationId, int id, int pageNumber, int pageSize, string searchString = null, string orderBy = null)
    {
        var result = await SecurityService.ApplicationRolesGet(ApplicationId, id, pageNumber, pageSize, searchString, orderBy);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationRoles.Edit}")]
    public async Task<IActionResult> Update(ApplicationRoleUpdateDto model)
    {
        var result = await SecurityService.ApplicationRoleUpdate(model);
        return Ok(result);
    }

    [HttpDelete]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationRoles.Delete}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await SecurityService.ApplicationRoleDelete(id);
        return Ok(result);
    }

    [HttpPost("IsEnabledToggle")]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationRoles.IsEnabledToggle}")]
    public async Task<IActionResult> IsEnabledToggle([FromBody] int id)
    {
        var result = await SecurityService.ApplicationRoleIsEnabledToggle(id);
        return Ok(result);
    }
}