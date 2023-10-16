using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DsK.AuthServer.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SecuredController : ControllerBase
	{
		[HttpGet]
		[Authorize(Roles = "admin,secured.permission1")]
		[Route("get")]
		public IActionResult Get()
		{	
			return Ok(new { message = "Hey i'm only for authorized users" });
		}

		[HttpGet]
		[Authorize(Roles = "admin,secured.permission2")]
		[Route("get-claims")]
		public IActionResult GetUserClaims()
		{
			List<string> userClaims = new List<string>();
			foreach (var claim in HttpContext.User.Claims)
			{
				userClaims.Add(claim.Value);
			}
			return Ok(userClaims);
		}

		[HttpGet]
		[Authorize(Roles = "admin,secured.permission3")]
		[Route("todos")]
		public IActionResult GetTodos()
		{	
			var todos = new List<string> { "Watch Movie", "Shoping", "Party" };
			return Ok(todos);
		}
	}
}
