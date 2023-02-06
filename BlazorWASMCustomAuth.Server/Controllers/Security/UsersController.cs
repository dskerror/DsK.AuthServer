using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Security.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security
{
    [Route("api/Security/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public UsersController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpPost]
        //[Authorize(Roles = "admin,UserCreate")]        
        public async Task<IActionResult> UserCreate(UserCreateDto model)
        {
            var result = await SecurityService.UserCreate(model);
            return Ok(result);
        }

        [HttpGet]
        //[Authorize(Roles = $"{Access.Admin}, {Access.Users.View}")]
        public async Task<IActionResult> UsersGet(int id, int pageNumber, int pageSize, string searchString = null, string orderBy = null)
        {
            var result = await SecurityService.UsersGet(id, pageNumber, pageSize, searchString, orderBy);
            return Ok(result);
        }

        [HttpPut]
        //[Authorize(Roles = "admin,UserDelete")]
        public async Task<IActionResult> UserUpdate(UserDto model)
        {
            var result = await SecurityService.UserUpdate(model);
            return Ok(result);
        }

        [HttpDelete]
        //[Authorize(Roles = "admin,UserDelete")]
        public async Task<IActionResult> UserDelete(int id)
        {
            var result = await SecurityService.UserDelete(id);
            return Ok(result);
        }

    }
}

