using AutoMapper;
using DsK.AuthServer.Security.Infrastructure;
using DsK.AuthServer.Security.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DsK.AuthServer.Server.Controllers.Security;
[Route("[controller]")]
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

    [HttpPost]
    [Authorize(Roles = $"{Access.Admin}, {Access.Users.Create}")]
    public async Task<IActionResult> Create(UserCreateDto model)
    {
        var result = await SecurityService.UserCreate(model);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = $"{Access.Admin}, {Access.Users.View}")]
    public async Task<IActionResult> Get([FromQuery] PagedRequest pagingRequest)
    {
        var result = await SecurityService.UsersGet(pagingRequest);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{Access.Admin}, {Access.Users.Edit}")]
    public async Task<IActionResult> Update(UserDto model)
    {
        var result = await SecurityService.UserUpdate(model);
        return Ok(result);
    }

    [HttpDelete]
    [Authorize(Roles = $"{Access.Admin}, {Access.Users.Delete}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await SecurityService.UserDelete(id);
        return Ok(result);
    }
}