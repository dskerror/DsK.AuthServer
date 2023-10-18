using DsK.AuthServer.Security.EntityFramework.Models;
using DsK.AuthServer.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace DsK.AuthServer.Security.Infrastructure;
public partial class SecurityService
{
    public async Task<APIResult<UserDto>> MyProfileGet(int id)
    {
        var result = new APIResult<UserDto>();
        var user = await db.Users.FindAsync(id);
        result.Result = Mapper.Map<User, UserDto>(user);
        return result;
    }
    public async Task<APIResult<string>> MyProfileUpdate(int userId, MyProfileUpdateDto model)
    {
        APIResult<string> result = new APIResult<string>();
        int recordsUpdated = 0;
        var record = await db.Users.FirstOrDefaultAsync(x => x.Id == userId);

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

    public async Task<bool> MyProfileChangePassword(int userId, MyProfileChangePasswordDto model)
    {
        int recordsUpdated = 0;
        var record = await db.Users.FirstOrDefaultAsync(x => x.Id == userId);

        //Password
        var ramdomSalt = SecurityHelpers.RandomizeSalt;
        record.HashedPassword = SecurityHelpers.HashPasword(model.Password, ramdomSalt);
        record.Salt = Convert.ToHexString(ramdomSalt);

        try
        {
            recordsUpdated = await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return false;
        }

        if (recordsUpdated == 1)
            return true;

        return false;
    }
}

