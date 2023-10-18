using DsK.AuthServer.Security.Infrastructure;
using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DsK.AuthServer.Server.Controllers.Security;
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
    public async Task<IActionResult> Get()
    {
        var userId = GetUserId();
        
        if (userId != 0)
        {
            var result = await SecurityService.MyProfileGet(userId);
            return Ok(result);
        } else
        {
            return NotFound("UserId not found");
        }
    }

    [HttpPut]
    [Authorize(Roles = $"{Access.Admin}, {Access.MyProfile.Edit}")]
    public async Task<IActionResult> Update(MyProfileUpdateDto model)
    {
        var userId = GetUserId();
        var result = await SecurityService.MyProfileUpdate(userId, model);
        return Ok(result);
    }

    [HttpPut("ChangePassword")]
    [Authorize(Roles = $"{Access.Admin}, {Access.MyProfile.Edit}")]
    public async Task<IActionResult> ChangePassword(MyProfileChangePasswordDto model)
    {
        var userId = GetUserId();
        var result = await SecurityService.MyProfileChangePassword(userId, model);
        return Ok(result);
    }

    private int GetUserId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            //IEnumerable<Claim> claims = identity.Claims;
            return int.Parse(identity.FindFirst("UserId").Value);
            
        }
        else        
            return 0;        
    }
}