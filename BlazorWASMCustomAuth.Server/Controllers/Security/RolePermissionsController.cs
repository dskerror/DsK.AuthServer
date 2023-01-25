using BlazorWASMCustomAuth.PagingSortingFiltering;
using BlazorWASMCustomAuth.Security.EntityFramework.Models;
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
        public IActionResult RolePermissionsGet(int roleid = 0)
        {
            return Ok(SecurityService.RolePermissionsGet(roleid));
        }


        [HttpPost]
        //[Authorize(Roles = "admin,RoleCreate")]
        public IActionResult RolePermissionCreate(RolePermissionCreateDto model)
        {
            var result = SecurityService.RolePermissionCreate(model);
            return Ok(result);
        }

        [HttpDelete]
        //[Authorize(Roles = "admin,RoleDelete")]
        public IActionResult RolePermissionDelete(RolePermissionDeleteDto model)
        {
            var result = SecurityService.RolePermissionDelete(model);
            return Ok(result);
        }
    }
}

