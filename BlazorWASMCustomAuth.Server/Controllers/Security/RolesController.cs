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
    public class RolesController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public RolesController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpGet]
        //[Authorize(Roles = "admin,RolesGet")]
        public IActionResult RolesGet()
        {
            return Ok(SecurityService.RolesGet());
        }

        [HttpPost]
        //[Authorize(Roles = "admin,RoleCreate")]
        public IActionResult RoleCreate(RoleCreateModel model)
        {
            var result = SecurityService.RoleCreate(model);
            return Ok(result);
        }

        [HttpPut]
        //[Authorize(Roles = "admin,RoleUpdate")]
        public IActionResult RoleUpdate(RoleModel model)
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

