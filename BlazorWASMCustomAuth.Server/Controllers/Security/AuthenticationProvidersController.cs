using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Security.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = $"{Access.Admin}, {Access.AuthenticationProvider.Create}")]
        public async Task<IActionResult> AuthenticationProvidersCreate(AuthenticationProviderCreateDto model)
        {
            var result = await SecurityService.AuthenticationProvidersCreate(model);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = $"{Access.Admin}, {Access.AuthenticationProvider.View}")]
        public async Task<IActionResult> AuthenticationProvidersGet(int id, int pageNumber, int pageSize, string searchString = null, string orderBy = null)
        {
            var result = await SecurityService.AuthenticationProvidersGet(id, pageNumber, pageSize, searchString, orderBy);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = $"{Access.Admin}, {Access.AuthenticationProvider.Edit}")]
        public async Task<IActionResult> AuthenticationProvidersUpdate(AuthenticationProviderUpdateDto model)
        {
            var result = await SecurityService.AuthenticationProvidersUpdate(model);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = $"{Access.Admin}, {Access.AuthenticationProvider.Delete}")]
        public async Task<IActionResult> AuthenticationProvidersDelete(int id)
        {
            var result = await SecurityService.AuthenticationProvidersDelete(id);
            return Ok(result);
        }
    }
}

