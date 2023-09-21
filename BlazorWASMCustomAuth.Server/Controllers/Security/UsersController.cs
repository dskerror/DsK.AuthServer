using AutoMapper;
using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMCustomAuth.Server.Controllers.Security;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly SecurityService SecurityService;
    private IMapper Mapper;

    public UsersController(SecurityService securityService, IMapper Mapper)
    {
        SecurityService = securityService;
        this.Mapper = Mapper;
    }

    //TODO : ChangePassword
    //TODO : Forgot Password
    
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register(UserRegisterDto model)
    {
        var record = new UserCreateDto();
        Mapper.Map(model, record);
        var result = await SecurityService.UserCreate(record);

        if (result.Result != null)
        {
            var newPassword = new UserCreateLocalPasswordDto()
            {
                Password = model.Password,
                UserId = result.Result.Id
            };
            await SecurityService.UserCreateLocalPassword(newPassword);
        }
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = $"{Access.Admin}, {Access.Users.Create}")]
    public async Task<IActionResult> UserCreate(UserCreateDto model)
    {
        var result = await SecurityService.UserCreate(model);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = $"{Access.Admin}, {Access.Users.View}")]
    public async Task<IActionResult> UsersGet(int id, int pageNumber, int pageSize, string searchString = null, string orderBy = null)
    {
        var result = await SecurityService.UsersGet(id, pageNumber, pageSize, searchString, orderBy);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{Access.Admin}, {Access.Users.Edit}")]
    public async Task<IActionResult> UserUpdate(UserDto model)
    {
        var result = await SecurityService.UserUpdate(model);
        return Ok(result);
    }

    [HttpDelete]
    [Authorize(Roles = $"{Access.Admin}, {Access.Users.Delete}")]
    public async Task<IActionResult> UserDelete(int id)
    {
        var result = await SecurityService.UserDelete(id);
        return Ok(result);
    }
}