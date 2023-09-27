using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BlazorWASMCustomAuth.Security.Infrastructure;
public partial class SecurityService
{
    public async Task<APIResult<UserDto>> MyProfileGet(int id)
    {
        var result = new APIResult<UserDto>();
        var user = await db.Users.FindAsync(id);
        result.Result = Mapper.Map<User, UserDto>(user);
        return result;
    }
    public async Task<APIResult<string>> MyProfileUpdate(UserDto model)
    {
        APIResult<string> result = new APIResult<string>();
        int recordsUpdated = 0;
        var record = await db.Users.FirstOrDefaultAsync(x => x.Id == model.Id);

        if (record != null)
            Mapper.Map(model, record);

        try
        {
            recordsUpdated = await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            result.HasError = true;
            result.Message = ex.InnerException.Message;
        }

        if (recordsUpdated == 1)
            result.Message = "Record Updated";

        return result;
    }
}

