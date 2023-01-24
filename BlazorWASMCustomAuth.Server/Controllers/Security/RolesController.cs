using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security
{
    [Route("api/Security/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public RolesController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpGet]
        //[Authorize(Roles = "admin,RolesGet")]
        public IActionResult RolesGet(int id = 0)
        {
            return Ok(SecurityService.RolesGet(id));
        }

        [HttpPost]
        //[Authorize(Roles = "admin,RoleCreate")]
        public IActionResult RoleCreate(RoleCreateDto model)
        {
            var result = SecurityService.RoleCreate(model);
            return Ok(result);
        }

        [HttpPut]
        //[Authorize(Roles = "admin,RoleUpdate")]
        public IActionResult RoleUpdate(RoleUpdateDto model)
        {
            var result = SecurityService.RoleUpdate(model);
            return Ok(result);
        }

        [HttpDelete]
        //[Authorize(Roles = "admin,RoleDelete")]
        public IActionResult RoleDelete(int id)
        {
            var result = SecurityService.RoleDelete(id);
            return Ok(result);
        }
    }
}

