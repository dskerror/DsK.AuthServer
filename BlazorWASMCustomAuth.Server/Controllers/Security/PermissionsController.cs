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
    public class PermissionsController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public PermissionsController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpGet]
        //[Authorize(Roles = "admin,PermissionsGet")]
        public IActionResult PermissionsGet(int id = 0)
        {
            return Ok(SecurityService.PermissionsGet(id));
        }

        [HttpPost]
        //[Authorize(Roles = "admin,PermissionCreate")]
        public IActionResult PermissionCreate(PermissionCreateDto model)
        {
            var result = SecurityService.PermissionCreate(model);
            return Ok(result);
        }
        [HttpPut]
        //[Authorize(Roles = "admin,PermissionUpdate")]
        public IActionResult PermissionUpdate(PermissionUpdateDto model)
        {
            var result = SecurityService.PermissionUpdate(model);
            return Ok(result);
        }

        [HttpDelete]
        //[Authorize(Roles = "admin,PermissionDelete")]
        public IActionResult PermissionDelete(int id)
        {
            var result = SecurityService.PermissionDelete(id);
            return Ok(result);
        }
    }
}

