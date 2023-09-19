using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security
{
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
        [Route("ApplicationLoginRequest")]
        public async Task<IActionResult> ApplicationLoginRequest(ApplicationLoginRequestDto model)
        {
            //todo : implement captcha
            var result = await SecurityService.ApplicationLoginRequest(model);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpPost]
        [Route("UserLogin")]
        public async Task<IActionResult> UserLogin(UserLoginDto model)
        {
            //todo : implement captcha
            var tokenModel = await SecurityService.UserLogin(model);

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
}

