using BlazorWASMCustomAuth.PagingSortingFiltering;
using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly SecurityService SecurityService;
        public SecurityController(SecurityService securityService)
        {
            SecurityService = securityService;
        }

        [HttpPost]
        [Route("UserLogin")]
        public IActionResult UserLogin(UserLoginModel model)
        {
            var tokenModel = SecurityService.UserLogin(model);

            if (tokenModel == null)
                return NotFound();

            return Ok(tokenModel);
        }

        [HttpPost]
        [Route("RefreshToken")]
        public IActionResult RefreshToken(TokenModel refreshToken)
        {
            var resultTokenModel = SecurityService.RefreshToken(refreshToken);
            if (refreshToken == null)
            {
                return NotFound();
            }
            return Ok(resultTokenModel);
        }

        [HttpPost]
        //[Authorize(Roles = "admin,UserCreate")]
        [Route("UserCreate")]
        public IActionResult UserCreate(UserCreateModel user)
        {
            return Ok(SecurityService.UserCreate(user));
        }

        [HttpPost]
        //[Authorize(Roles = "admin,UserCreateLocalPassword")]
        [Route("UserCreateLocalPassword")]
        public IActionResult UserCreateLocalPassword(UserCreateLocalPasswordModel userCreateLocalPasswordModel)
        {
            return Ok(SecurityService.UserCreateLocalPassword(userCreateLocalPasswordModel));
        }

        [HttpPost]
        //[Authorize(Roles = "admin,UserChangeLocalPassword")]
        [Route("UserChangeLocalPassword")]
        public IActionResult UserChangeLocalPassword(UserChangeLocalPasswordModel userChangeLocalPasswordModel)
        {
            return Ok(SecurityService.UserChangeLocalPassword(userChangeLocalPasswordModel));
        }

        [HttpGet]
        //[Authorize(Roles = "admin,UsersGet")]
        [Route("UsersGet")]
        public IActionResult UsersGet([FromQuery] PagingSortingFilteringRequest request)
        {
            return Ok(SecurityService.UsersGet(request));
        }

        //[HttpGet]
        //[Authorize(Roles = "admin,UserGet")]
        //[Route("UserGet")]
        //public IActionResult UserGet([FromQuery] int id)
        //{
        //    return Ok(SecurityService.UserGet(id));
        //}

        [HttpGet]
        //[Authorize(Roles = "admin,PermissionsGet")]
        [Route("PermissionsGet")]
        public IActionResult PermissionsGet()
        {
            return Ok(SecurityService.PermissionsGet());
        }

        [HttpGet]
        //[Authorize(Roles = "admin,RolesGet")]
        [Route("RolesGet")]
        public IActionResult RolesGet()
        {
            return Ok(SecurityService.RolesGet());
        }

        [HttpGet]
        //[Authorize(Roles = "admin,RolePermissionsGet")]
        [Route("RolePermissionsGet")]
        public IActionResult RolePermissionsGet()
        {
            return Ok(SecurityService.RolePermissionsGet());
        }
    }
}
