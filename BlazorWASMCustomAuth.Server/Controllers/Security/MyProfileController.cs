using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security;
[Route("api/[controller]")]
[ApiController]
public class MyProfileController : ControllerBase
{
    private readonly SecurityService SecurityService;    

    public MyProfileController(SecurityService securityService)
    {
        SecurityService = securityService;
    }

    [HttpGet]
    [Authorize(Roles = $"{Access.Admin}, {Access.MyProfile.View}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await SecurityService.MyProfileGet(id);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{Access.Admin}, {Access.MyProfile.Edit}")]
    public async Task<IActionResult> Update(UserDto model)
    {
        var result = await SecurityService.MyProfileUpdate(model);
        return Ok(result);
    }
}