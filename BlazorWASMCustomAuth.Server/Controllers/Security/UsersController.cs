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
    public class UsersController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public UsersController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpPost]
        //[Authorize(Roles = "admin,UserCreate")]
        //[Route("UserCreate")]
        public IActionResult UserCreate(UserCreateDto model)
        {
            var result = SecurityService.UserCreate(model);
            return Ok(result);
        }

        [HttpGet]
        //[Authorize(Roles = "admin,UsersGet")]
        //[Route("UsersGet")]
        public IActionResult UsersGet([FromQuery] PagingSortingFilteringRequest request)
        {
            return Ok(SecurityService.UsersGet(request));
        }

        [HttpGet]
        //[Authorize(Roles = "admin,UsersGet")]
        [Route("UsersGetNew")]
        public IActionResult UsersGetNew([FromQuery] PagingSortingFilteringRequest model)
        {
            return Ok(SecurityService.UsersGetNew(model));
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
    

        [HttpGet]
        //[Authorize(Roles = "admin,UserUserVerifyExistsByUsernamesGet")]
        [Route("VerifyExistsByUsername")]
        public IActionResult UserVerifyExistsByUsername([FromQuery] string username)
        {
            return Ok(SecurityService.UserVerifyExistsByUsername(username));
        }

        [HttpGet]
        //[Authorize(Roles = "admin,UserUserVerifyExistsByUsernamesGet")]
        [Route("VerifyExistsByEmail")]
        public IActionResult UserVerifyExistsByEmail([FromQuery] string email)
        {
            return Ok(SecurityService.UserVerifyExistsByEmail(email));
        }

     
    }
}

