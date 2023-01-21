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
    public class RolePermissionsController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public RolePermissionsController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpGet]
        //[Authorize(Roles = "admin,RolePermissionsGet")]        
        public IActionResult RolePermissionsGet()
        {
            return Ok(SecurityService.RolePermissionsGet());
        }


        [HttpPost]
        //[Authorize(Roles = "admin,RoleCreate")]
        public IActionResult RolePermissionCreate(RolePermissionDto model)
        {
            var result = SecurityService.RolePermissionCreate(model);
            return Ok(result);
        }

        [HttpDelete]
        //[Authorize(Roles = "admin,RoleDelete")]
        public IActionResult RolePermissionDelete(RolePermissionDto model)
        {
            var result = SecurityService.RolePermissionDelete(model);
            return Ok(result);
        }
    }
}

