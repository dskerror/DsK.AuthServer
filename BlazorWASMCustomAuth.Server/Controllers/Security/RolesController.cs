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

        [HttpPost]
        //[Authorize(Roles = "admin,RoleCreate")]
        public IActionResult RoleCreate(RoleCreateDto model)
        {
            var result = SecurityService.RoleCreate(model);
            return Ok(result);
        }

        [HttpGet]
        //[Authorize(Roles = "admin,RolesGet")]
        public IActionResult RolesGet(int id, int pageNumber, int pageSize, string searchString = null, string orderBy = null)
        {
            return Ok(SecurityService.RolesGet(id, pageNumber, pageSize, searchString, orderBy));
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

