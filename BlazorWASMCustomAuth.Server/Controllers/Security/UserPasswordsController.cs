using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security
{
    [Route("api/Security/[controller]")]
    [ApiController]
    public class UserPasswordsController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public UserPasswordsController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpPost]
        //[Authorize(Roles = "admin,UserCreateLocalPassword")]
        [Route("CreateLocalPassword")]
        public async Task<IActionResult> UserCreateLocalPassword(UserCreateLocalPasswordDto model)
        {
            //TODO : Create another method for user to change their own passwords
            var result = await SecurityService.UserCreateLocalPassword(model);
            return Ok(result);
        }
     
    }
}

