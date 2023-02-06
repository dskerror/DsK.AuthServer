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
        public async Task<IActionResult> AuthenticationProvidersCreate(AuthenticationProviderCreateDto model)
        {
            var result = await SecurityService.AuthenticationProvidersCreate(model);
            return Ok(result);
        }

        [HttpGet]
        //[Authorize(Roles = "admin,RolesGet")]
        public async Task<IActionResult> AuthenticationProvidersGet(int id, int pageNumber, int pageSize, string searchString = null, string orderBy = null)
        {
            var result = SecurityService.AuthenticationProvidersGet(id, pageNumber, pageSize, searchString, orderBy);
            return Ok(result);
        }

        [HttpPut]
        //[Authorize(Roles = "admin,RoleUpdate")]
        public async Task<IActionResult> AuthenticationProvidersUpdate(AuthenticationProviderUpdateDto model)
        {
            var result = await SecurityService.AuthenticationProvidersUpdate(model);
            return Ok(result);
        }

        [HttpDelete]
        //[Authorize(Roles = "admin,RoleDelete")]
        public async Task<IActionResult> AuthenticationProvidersDelete(int id)
        {
            var result = await SecurityService.AuthenticationProvidersDelete(id);
            return Ok(result);
        }
    }
}

