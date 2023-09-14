using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Security.Shared.Constants;
using BlazorWASMCustomAuth.Security.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BlazorWASMCustomAuth.Server.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationRolesController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public ApplicationRolesController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpPost]
        [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationRoles.Create}")]
        public async Task<IActionResult> RoleCreate(ApplicationRoleCreateDto model)
        {
            var result = await SecurityService.RoleCreate(model);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationRoles.View}")]
        public async Task<IActionResult> RolesGet(int ApplicationId, int id, int pageNumber, int pageSize, string searchString = null, string orderBy = null)
        {            
            var result = await SecurityService.ApplicationRolesGet(ApplicationId, id, pageNumber, pageSize, searchString, orderBy);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationRoles.Edit}")]
        public async Task<IActionResult> RoleUpdate(ApplicationRoleUpdateDto model)
        {
            var result = await SecurityService.RoleUpdate(model);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = $"{Access.Admin}, {Access.ApplicationRoles.Delete}")]
        public async Task<IActionResult> RoleDelete(int id)
        {
            var result = await SecurityService.RoleDelete(id);
            return Ok(result);
        }
    }
}

