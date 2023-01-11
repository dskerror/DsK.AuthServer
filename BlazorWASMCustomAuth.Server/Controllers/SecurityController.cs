using BlazorWASMCustomAuth.PagingSortingFiltering;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Server.Models;
using BlazorWASMCustomAuth.Server.Services;
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

        public SecurityController(SecurityService accountLogic)
        {
            SecurityService = accountLogic;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(UserLoginModel model)
        {
            var tokenModel = SecurityService.LoginUser(model);

            if (tokenModel == null)
                return NotFound();

            return Ok(tokenModel);

        }

        //[HttpPost]
        //[Route("activate-token-by-refreshtoken")]
        //public IActionResult ActivateAccessTokenByRefresh(TokenModel refreshToken)
        //{
        //	var resultTokenModel = SecurityService.ActivateTokenUsingRefreshToken(refreshToken);
        //	if (refreshToken == null)
        //	{
        //		return NotFound();
        //	}
        //	return Ok(resultTokenModel);
        //}


        [HttpPost]
        [Route("UserCreate")]
        public IActionResult UserCreate(UserCreateModel user)
        {
            return Ok(SecurityService.UserCreate(user));
        }

        [HttpPost]
        [Route("UserCreateLocalPassword")]
        public IActionResult UserCreateLocalPassword(UserCreateLocalPasswordModel userCreateLocalPasswordModel)
        {
            return Ok(SecurityService.UserCreateLocalPassword(userCreateLocalPasswordModel));
        }

		[HttpGet]
        //[Authorize(Roles = "admin,UserGet")]
        [Route("UsersGet")]
        public IActionResult UsersGet([FromQuery] PagingSortingFilteringRequest request)
        {
            return Ok(SecurityService.UsersGet(request));
        }


        [HttpGet]
        [Route("PermissionList")]
        public IActionResult PermissionList()
        {
            return Ok(SecurityService.GetPermissionList());
        }

        [HttpGet]
        [Route("RoleList")]
        public IActionResult RoleList()
        {
            return Ok(SecurityService.GetRoleList());
        }

        [HttpGet]
        [Route("RolePermissionList")]
        public IActionResult RolePermissionList()
        {
            return Ok(SecurityService.RolePermissionList());
        }
    }
}
