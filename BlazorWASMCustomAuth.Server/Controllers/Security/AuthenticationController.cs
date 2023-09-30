using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Security.Shared.ActionDtos;
using Microsoft.AspNetCore.Mvc;
namespace BlazorWASMCustomAuth.Server.Controllers.Security;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly SecurityService SecurityService;
    public AuthenticationController(SecurityService securityService)
    {
        SecurityService = securityService;
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(LoginRequestDto model)
    {
        //todo : implement captcha
        var callbackUrl = await SecurityService.Login(model);

        if (callbackUrl == null)
            return NotFound();

        return Ok(callbackUrl);
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register(RegisterRequestDto model)
    {
        //todo : implement captcha
        var origin = Request.Headers["origin"];
        var registerSuccessfull = await SecurityService.Register(model);
        return Ok(registerSuccessfull);
    }

    [HttpPost]
    [Route("PasswordChangeRequest")]
    public async Task<IActionResult> PasswordChangeRequest(PasswordChangeRequestDto model)
    {
        //todo : implement captcha
        var status = await SecurityService.PasswordChangeRequest(model);
        return Ok(status);
    }

    [HttpPost]
    [Route("PasswordChange")]
    public async Task<IActionResult> PasswordChange(PasswordChangeDto model)
    {
        //todo : implement captcha
        var status = await SecurityService.PasswordChange(model);
        return Ok(status);
    }

    [HttpPost]
    [Route("ValidateLoginToken")]
    public async Task<IActionResult> ValidateLoginToken(ValidateLoginTokenDto model)
    {   
        var tokenModel = await SecurityService.ValidateLoginToken(model);

        if (tokenModel == null)
            return NotFound();

        return Ok(tokenModel);
    }

    [HttpPost]
    [Route("RefreshToken")]
    public async Task<IActionResult> RefreshToken(TokenModel refreshToken)
    {
        var resultTokenModel = await SecurityService.RefreshToken(refreshToken);
        if (resultTokenModel == null)
        {
            return NotFound();
        }
        return Ok(resultTokenModel);
    }
}

