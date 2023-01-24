using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security
{
    [Route("api/Security/[controller]")]
    [ApiController]
    public class AuthenticationProvidersController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public AuthenticationProvidersController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpGet]
        //[Authorize(Roles = "admin,RolesGet")]
        public IActionResult AuthenticationProvidersGet(int id = 0)
        {
            return Ok(SecurityService.AuthenticationProvidersGet(id));
        }

        [HttpPost]
        //[Authorize(Roles = "admin,RoleCreate")]
        public IActionResult AuthenticationProvidersCreate(AuthenticationProvider model)
        {
            var result = SecurityService.AuthenticationProvidersCreate(model);
            return Ok(result);
        }

        [HttpPut]
        //[Authorize(Roles = "admin,RoleUpdate")]
        public IActionResult AuthenticationProvidersUpdate(AuthenticationProvider model)
        {
            var result = SecurityService.AuthenticationProvidersUpdate(model);
            return Ok(result);
        }

        [HttpDelete]
        //[Authorize(Roles = "admin,RoleDelete")]
        public IActionResult AuthenticationProvidersDelete(int id)
        {
            var result = SecurityService.AuthenticationProvidersDelete(id);
            return Ok(result);
        }
    }
}

