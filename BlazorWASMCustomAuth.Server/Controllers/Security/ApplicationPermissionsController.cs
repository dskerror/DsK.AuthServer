using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security;

[Route("api/[controller]")]
[ApiController]
public class ApplicationPermissionsController : ControllerBase
{
    private readonly SecurityService SecurityService;
    public ApplicationPermissionsController(SecurityService securityService)
    {
        SecurityService = securityService;
    }

    //[HttpGet]
    //[Authorize(Roles = $"{Access.Admin}, {Access.ApplicationPermissions.View}")]
    //public async Task<IActionResult> ApplicationPermissionsGet(int applicationId)
    //{
    //    var result = await SecurityService.ApplicationPermissionsGet(applicationId);
    //    return Ok(result);
    //}

    [HttpGet]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationPermissions.View}")]
    public async Task<IActionResult> Get(int ApplicationId, int id, int pageNumber, int pageSize, string searchString = null, string orderBy = null)
    {
        var result = await SecurityService.ApplicationPermissionsGet(ApplicationId, id, pageNumber, pageSize, searchString, orderBy);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationPermissions.Create}")]
    public async Task<IActionResult> Create(ApplicationPermissionCreateDto model)
    {
        var result = await SecurityService.ApplicationPermissionCreate(model);
        return Ok(result);
    }
    [HttpPut]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationPermissions.Edit}")]
    public async Task<IActionResult> Update(ApplicationPermissionUpdateDto model)
    {
        var result = await SecurityService.ApplicationPermissionUpdate(model);
        return Ok(result);
    }

    [HttpDelete]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationPermissions.Delete}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await SecurityService.ApplicationPermissionDelete(id);
        return Ok(result);
    }

    [HttpPost("DisableEnable")]
    [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationPermissions.DisableEnable}")]
    public async Task<IActionResult> DisableEnable([FromBody] int id)
    {
        var result = await SecurityService.ApplicationPermissionDisableEnabled(id);
        return Ok(result);
    }
}

