﻿using BlazorWASMCustomAuth.Security.EntityFramework.Models;
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
        public async Task<IActionResult> RolePermissionsGet(int roleid)
        {
            if(roleid == 0)            
                return BadRequest();

            var result = await SecurityService.RolePermissionsGet(roleid);
            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Roles = "admin,RoleCreate")]
        public async Task<IActionResult> RolePermissionChange(RolePermissionChangeDto model)
        {
            var result = await SecurityService.RolePermissionChange(model);
            return Ok(result);
        }
    }
}

