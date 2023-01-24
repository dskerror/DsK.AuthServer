using BlazorWASMCustomAuth.PagingSortingFiltering;
using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security
{
    [Route("api/Security/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public AuthenticationController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpPost]
        [Route("UserLogin")]
        public IActionResult UserLogin(UserLoginDto model)
        {
            //implement captcha
            var tokenModel = SecurityService.UserLogin(model);

            if (tokenModel == null)
                return NotFound();

            return Ok(tokenModel);
        }

        [HttpPost]
        [Route("RefreshToken")]
        public IActionResult RefreshToken(TokenModel refreshToken)
        {
            var resultTokenModel = SecurityService.RefreshToken(refreshToken);
            if (resultTokenModel == null)
            {
                return NotFound();
            }
            return Ok(resultTokenModel);
        }
    }
}

