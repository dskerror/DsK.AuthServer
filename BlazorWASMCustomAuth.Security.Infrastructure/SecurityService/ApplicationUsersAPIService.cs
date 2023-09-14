using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;


namespace BlazorWASMCustomAuth.Security.Infrastructure;
public partial class SecurityService
{
    public async Task<APIResult<UserDto>> ApplicationUserCreate(UserCreateDto model)
    {
        APIResult<UserDto> result = new APIResult<UserDto>();
        int recordsCreated = 0;

        var record = new User();
        Mapper.Map(model, record);
        record.EmailConfirmed = true;
        await db.Users.AddAsync(record);

        try
        {
            recordsCreated = await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            result.HasError = true;
            result.Message = ex.InnerException.Message;
        }

        var userRole = new UserRole()
        {
            RoleId = 2,
            UserId = record.Id
        };
        await db.UserRoles.AddAsync(userRole);

        var userAuthenticationProviderMapping = new UserAuthenticationProviderMapping()
        {

            UserId = record.Id,
            Username = record.Email,
        };

        await db.UserAuthenticationProviderMappings.AddAsync(userAuthenticationProviderMapping);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            result.HasError = true;
            result.Message = ex.InnerException.Message;
        }

        if (recordsCreated == 1)
        {
            result.Result = Mapper.Map(record, result.Result);
            result.Message = "Record Created";
        }

        return result;
    }
    public async Task<APIResult<List<ApplicationUserDto>>> ApplicationUsersGet(int applicationId, int id, int pageNumber, int pageSize, string searchString, string orderBy)
    {
        var result = new APIResult<List<ApplicationUserDto>>();

        string ordering = "Id";
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            string[] OrderBy = orderBy.Split(',');
            ordering = string.Join(",", OrderBy);
        }
        result.Paging.CurrentPage = pageNumber;
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 10 : pageSize;
        int count = 0;
        List<ApplicationUser> items;
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            count = await db.Users
                .Where(m => m.Name.Contains(searchString) || m.Email.Contains(searchString))
                .CountAsync();

            items = await db.ApplicationUsers.OrderBy(ordering)
                //.Where(m => m.Name.Contains(searchString) || m.Email.Contains(searchString))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        else if (id > 0)
        {
            count = await db.ApplicationUsers
                .Where(u => u.Id == id)
                .CountAsync();

            items = await db.ApplicationUsers.OrderBy(ordering)
                .Where(u => u.Id == id)
                .ToListAsync();
        }
        else if (id == -1)
        {
            count = await db.ApplicationUsers.CountAsync();
            items = await db.ApplicationUsers.ToListAsync();
        }
        else
        {
            count = await db.ApplicationUsers.CountAsync();

            items = await db.ApplicationUsers
                .Where(u => u.ApplicationId == applicationId)
                .Include(x => x.User)
                .OrderBy(ordering)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        result.Paging.TotalItems = count;
        result.Result = Mapper.Map<List<ApplicationUser>, List<ApplicationUserDto>>(items);
        return result;
    }
    public async Task<APIResult<string>> ApplicationUserUpdate(UserDto model)
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
    public async Task<APIResult<string>> ApplicationUserDelete(int id)
    {
        APIResult<string> result = new APIResult<string>();
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

    private async Task<User> ApplicationGetUserByEmailAsync(string username)
    {
        return await db.Users.Where(u => u.Email == username).FirstOrDefaultAsync();
    }

    private async Task<User> ApplicationGetUserByMappedUsernameAsync(string username, int AuthenticationProviderId)
    {
        var user = await (from u in db.Users
                          join uap in db.UserAuthenticationProviderMappings on u.Id equals uap.UserId
                          where uap.Username == username
                          select u).FirstOrDefaultAsync();
        return user;
    }
}

