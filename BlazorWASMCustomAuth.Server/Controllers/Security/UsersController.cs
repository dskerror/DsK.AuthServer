using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
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
        public IActionResult UserCreate(UserCreateDto model)
        {
            var result = SecurityService.UserCreate(model);
            return Ok(result);
        }

        [HttpGet]
        //[Authorize(Roles = "admin,UsersGet")]
        public IActionResult UsersGet(int id = 0)
        {
            return Ok(SecurityService.UsersGet(id));
        }

        [HttpPut]
        //[Authorize(Roles = "admin,UserDelete")]
        public IActionResult UserUpdate(UserUpdateDto model)
        {
            var result = SecurityService.UserUpdate(model);
            return Ok(result);
        }

        [HttpDelete]
        //[Authorize(Roles = "admin,UserDelete")]
        public IActionResult UserDelete(int id)
        {
            var result = SecurityService.UserDelete(id);
            return Ok(result);
        }

    }
}

