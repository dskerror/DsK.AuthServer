using DsK.AuthServer.Security.EntityFramework.Models;
using DsK.AuthServer.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace DsK.AuthServer.Security.Infrastructure;
public partial class SecurityService
{
    public async Task<APIResponse<UserDto>> UserCreate(UserCreateDto model)
    {
        APIResponse<UserDto> result = new APIResponse<UserDto>();
        int recordsCreated = 0;

        var record = new User();
        Mapper.Map(model, record);

        //Password
        var ramdomSalt = SecurityHelpers.RandomizeSalt;
        record.HashedPassword = SecurityHelpers.HashPasword(model.Password, ramdomSalt);
        record.Salt = Convert.ToHexString(ramdomSalt);

        record.EmailConfirmed = true;
        record.AccountCreatedDateTime = DateTime.Now;
        record.EmailConfirmed = true;
        record.LastPasswordChangeDateTime = DateTime.Now;

        if (record.Id == 1)
            record.IsEnabled = true;

        await db.Users.AddAsync(record);

        try
        {
            recordsCreated = await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            result.HasError = true;
            result.Message = ex.InnerException.Message;
            return result;
        }

        if (recordsCreated == 1)
        {
            result.Result = Mapper.Map(record, result.Result);
            result.Message = "Record Created";
        }

        return result;
    }
    public async Task<APIResponse<List<UserDto>>> UsersGet(PagedRequest p)
    {
        var result = new APIResponse<List<UserDto>>();

        result.Paging.CurrentPage = p.PageNumber;
        p.PageNumber = p.PageNumber == 0 ? 1 : p.PageNumber;
        p.PageSize = p.PageSize == 0 ? 10 : p.PageSize;

        int count = 0;
        List<User> items;

        if (!string.IsNullOrWhiteSpace(p.SearchString))
        {
            count = await db.Users
                .Where(m => m.Name.Contains(p.SearchString) || m.Email.Contains(p.SearchString))
                .CountAsync();

            items = await db.Users.OrderBy(p.OrderBy)
                .Where(m => m.Name.Contains(p.SearchString) || m.Email.Contains(p.SearchString))
                .Skip((p.PageNumber - 1) * p.PageSize)
                .Take(p.PageSize)
                .ToListAsync();
        }
        else if (p.Id != 0)
        {
            count = await db.Users
                .Where(u => u.Id == p.Id)
                .CountAsync();

            items = await db.Users.OrderBy(p.OrderBy)
                .Where(u => u.Id == p.Id)
                .ToListAsync();
        }
        else
        {
            count = await db.Users.CountAsync();

            items = await db.Users.OrderBy(p.OrderBy)
                .Skip((p.PageNumber - 1) * p.PageSize)
                .Take(p.PageSize)
                .ToListAsync();
        }
        result.Paging.TotalItems = count;
        result.Result = Mapper.Map<List<User>, List<UserDto>>(items);
        return result;
    }
    public async Task<APIResponse<string>> UserUpdate(UserDto model)
    {
        APIResponse<string> result = new APIResponse<string>();
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
    public async Task<APIResponse<string>> UserDelete(int id)
    {
        APIResponse<string> result = new APIResponse<string>();
        int recordsDeleted = 0;
        var record = db.Users.Attach(new User { Id = id });
        record.State = EntityState.Deleted;
        try
        {
            recordsDeleted = await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            result.HasError = true;
            result.Message = ex.Message;
        }

        result.Result = recordsDeleted.ToString();

        return result;
    }
    private async Task<User> GetUserByEmailAsync(string username)
    {
        return await db.Users.Where(u => u.Email == username).FirstOrDefaultAsync();
    }
    private async Task<User> GetUserByMappedUsernameAsync(string username, int applicationAuthenticationProviderId)
    {
        try
        {
            var applicationAuthenticationProviderUserMapping = await db.ApplicationAuthenticationProviderUserMappings.Where(x => x.Username == username && x.ApplicationAuthenticationProviderId == applicationAuthenticationProviderId).SingleOrDefaultAsync();
            var applicationUser = await db.ApplicationUsers.FindAsync(applicationAuthenticationProviderUserMapping.ApplicationUserId);
            var user = await db.Users.FindAsync(applicationUser.UserId);

            //await (from u in db.Users
            //        join aapum in db.ApplicationAuthenticationProviderUserMappings on u.Id equals aapum.UserId
            //        where aapum.Username == username && aapum.ApplicationAuthenticationProviderId == applicationAuthenticationProviderId
            //        select u).FirstOrDefaultAsync();
            return user;
        }
        catch (Exception ex)
        {
            return null;
        }

    }
}