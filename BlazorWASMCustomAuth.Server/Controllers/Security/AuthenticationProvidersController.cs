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

        [HttpPost]
        //[Authorize(Roles = "admin,RoleCreate")]
        public IActionResult AuthenticationProvidersCreate(AuthenticationProviderCreateDto model)
        {
            var result = SecurityService.AuthenticationProvidersCreate(model);
            return Ok(result);
        }

        [HttpGet]
        //[Authorize(Roles = "admin,RolesGet")]
        public IActionResult AuthenticationProvidersGet(int id, int pageNumber, int pageSize, string searchString = null, string orderBy = null)
        {
            return Ok(SecurityService.AuthenticationProvidersGet(id, pageNumber, pageSize, searchString, orderBy));
        }

        [HttpPut]
        //[Authorize(Roles = "admin,RoleUpdate")]
        public IActionResult AuthenticationProvidersUpdate(AuthenticationProviderUpdateDto model)
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

