using AutoMapper;
using DsK.AuthServer.Security.Infrastructure;
using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DsK.AuthServer.Server.Controllers.Security;

[Route("[controller]")]
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
    public async Task<IActionResult> Create(ApplicationCreateDto model)
    {

        APIResponse<ApplicationDto> apiResult = new APIResponse<ApplicationDto>();
        try
        {

            var serviceResult = await SecurityService.ApplicationCreate(model);
            apiResult.Result = serviceResult;
            apiResult.Message = "Record Created";
        }
        catch (Exception ex)
        {
            apiResult.HasError = true;
            apiResult.Exception = ex;
            if (ex.InnerException.Message != null)
                apiResult.Message = ex.InnerException.Message;
        }

        return Ok(apiResult);
    }

    [HttpPost("GenerateNewAPIKey")]
    [Authorize(Roles = $"{Access.Admin}, {Access.Application.GenerateNewAPIKey}")]
    public async Task<IActionResult> GenerateNewAPIKey(ApplicationDto model)
    {
        var result = await SecurityService.ApplicationGenerateNewAPIKey(model.Id);
        return Ok(result);
    }

    [HttpPost("IsEnabledToggle")]
    [Authorize(Roles = $"{Access.Admin}, {Access.Application.IsEnabledToggle}")]
    public async Task<IActionResult> IsEnabledToggle([FromBody] int id)
    {
        var result = await SecurityService.ApplicationIsEnabledToggle(id);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = $"{Access.Admin}, {Access.Application.View}")]
    public async Task<IActionResult> Get([FromQuery] PagedRequest pagingRequest)
    {
        var result = await SecurityService.ApplicationGet(pagingRequest);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{Access.Admin}, {Access.Application.Edit}")]
    public async Task<IActionResult> Update(ApplicationUpdateDto model)
    {
        var result = await SecurityService.ApplicationUpdate(model);
        return Ok(result);
    }

    //[HttpDelete]
    //[Authorize(Roles = $"{Access.Admin}, {Access.Application.Delete}")]
    //public async Task<IActionResult> Delete(int id)
    //{
    //    var result = await SecurityService.ApplicationDelete(id);
    //    return Ok(result);
    //}
}

