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
            //implement captcha
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
            if (resultTokenModel == null)
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
            var result = SecurityService.UserCreate(user);
            return Ok(result);            
        }

        [HttpPost]
        //[Authorize(Roles = "admin,UserCreateLocalPassword")]
        [Route("UserCreateLocalPassword")]
        public IActionResult UserCreateLocalPassword(UserCreateLocalPasswordModel userCreateLocalPasswordModel)
        {
            //TODO : Create another method for user to change their own passwords
            return Ok(SecurityService.UserCreateLocalPassword(userCreateLocalPasswordModel));
        }

        [HttpGet]
        //[Authorize(Roles = "admin,UsersGet")]
        [Route("UsersGet")]
        public IActionResult UsersGet([FromQuery] PagingSortingFilteringRequest request)
        {
            return Ok(SecurityService.UsersGet(request));
        }

        [HttpGet]
        //[Authorize(Roles = "admin,UserUserVerifyExistsByUsernamesGet")]
        [Route("UserVerifyExistsByUsername")]
        public IActionResult UserVerifyExistsByUsername([FromQuery] string username)
        {
            return Ok(SecurityService.UserVerifyExistsByUsername(username));
        }

        [HttpGet]
        //[Authorize(Roles = "admin,UserUserVerifyExistsByUsernamesGet")]
        [Route("UserVerifyExistsByEmail")]
        public IActionResult UserVerifyExistsByEmail([FromQuery] string email)
        {
            return Ok(SecurityService.UserVerifyExistsByEmail(email));
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

        [HttpPost]
        //[Authorize(Roles = "admin,PermissionCreate")]
        [Route("PermissionCreate")]
        public IActionResult PermissionCreate(PermissionCreateModel model)
        {
            var result = SecurityService.PermissionCreate(model);
            return Ok(result);
        }

        [HttpGet]
        //[Authorize(Roles = "admin,RolesGet")]
        [Route("RolesGet")]
        public IActionResult RolesGet()
        {
            return Ok(SecurityService.RolesGet());
        }

        [HttpPost]
        //[Authorize(Roles = "admin,RoleCreate")]
        [Route("RoleCreate")]
        public IActionResult RoleCreate(RoleCreateModel model)
        {
            var result = SecurityService.RoleCreate(model);
            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Roles = "admin,RoleCreate")]
        [Route("RoleUpate")]
        public IActionResult RoleUpate(RoleModel model)
        {
            var result = SecurityService.RoleUpdate(model);
            return Ok(result);
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
