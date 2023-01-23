using BlazorWASMCustomAuth.PagingSortingFiltering;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.SecurityEF.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security
{
    [Route("api/Security/[controller]")]
    [ApiController]
    public class UsersEFController : ControllerBase
    {
        private readonly SecurityServiceEF SecurityService;
        public UsersEFController(SecurityServiceEF securityService)
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
        public IActionResult UsersGet()
        {
            return Ok(SecurityService.UsersGet());
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

