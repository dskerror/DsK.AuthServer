using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using static BlazorWASMCustomAuth.Security.Shared.Access;


namespace BlazorWASMCustomAuth.Security.Infrastructure;
public partial class SecurityService
{
    //public async Task<APIResult<UserDto>> ApplicationUserCreate(UserCreateDto model)
    //{
    //    APIResult<UserDto> result = new APIResult<UserDto>();
    //    int recordsCreated = 0;

    //    var record = new User();
    //    Mapper.Map(model, record);
    //    record.EmailConfirmed = true;
    //    await db.Users.AddAsync(record);

    //    try
    //    {
    //        recordsCreated = await db.SaveChangesAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        result.HasError = true;
    //        result.Message = ex.InnerException.Message;
    //    }

    //    var userRole = new UserRole()
    //    {
    //        RoleId = 2,
    //        UserId = record.Id
    //    };
    //    await db.UserRoles.AddAsync(userRole);

    //    var applicationAuthenticationProviderUserMapping = new ApplicationAuthenticationProviderUserMapping()
    //    {

    //        UserId = record.Id,
    //        Username = record.Email,
    //    };

    //    await db.ApplicationAuthenticationProviderUserMappings.AddAsync(applicationAuthenticationProviderUserMapping);

    //    try
    //    {
    //        await db.SaveChangesAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        result.HasError = true;
    //        result.Message = ex.InnerException.Message;
    //    }

    //    if (recordsCreated == 1)
    //    {
    //        result.Result = Mapper.Map(record, result.Result);
    //        result.Message = "Record Created";
    //    }

    //    return result;
    //}
    public async Task<APIResult<List<ApplicationUserGridDto>>> ApplicationUsersGet(int applicationId)
    {
        APIResult<List<ApplicationUserGridDto>> result = new APIResult<List<ApplicationUserGridDto>>();

        var users = await db.Users.ToListAsync();

        var applicationsUsers = await (from au in db.ApplicationUsers
                                      join u in db.Users on au.UserId equals u.Id
                                      where au.ApplicationId == applicationId
                                      select new { u.Name, u.Email, au.UserId, au.ApplicationId }).ToListAsync();

        List<ApplicationUserGridDto> gridDto = new List<ApplicationUserGridDto>();

        foreach (var user in users)
        {
            var value = new ApplicationUserGridDto()
            {
                ApplicationId = applicationId,
                Email = user.Email,
                Name = user.Name,
                UserId = user.Id
            };

            var lookupInApplicationsUsers = applicationsUsers.Where(x => x.UserId == user.Id && x.ApplicationId == applicationId).FirstOrDefault();
            if (lookupInApplicationsUsers != null) { value.IsEnabled = true; }

            gridDto.Add(value);
        }

        result.Result = gridDto;
        return result;
    }
    //public async Task<APIResult<string>> ApplicationUserUpdate(UserDto model)
    //{
    //    APIResult<string> result = new APIResult<string>();
    //    int recordsUpdated = 0;
    //    var record = await db.Users.FirstOrDefaultAsync(x => x.Id == model.Id);

    //    if (record != null)
    //        Mapper.Map(model, record);

    //    try
    //    {
    //        recordsUpdated = await db.SaveChangesAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        result.HasError = true;
    //        result.Message = ex.InnerException.Message;
    //    }

    //    if (recordsUpdated == 1)
    //        result.Message = "Record Updated";

    //    return result;
    //}
    //public async Task<APIResult<string>> ApplicationUserDelete(int id)
    //{
    //    APIResult<string> result = new APIResult<string>();
    //    int recordsDeleted = 0;
    //    var record = db.Users.Attach(new User { Id = id });
    //    record.State = EntityState.Deleted;
    //    try
    //    {
    //        recordsDeleted = await db.SaveChangesAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        result.HasError = true;
    //        result.Message = ex.Message;
    //    }

    //    result.Result = recordsDeleted.ToString();

    //    return result;
    //}
    //private async Task<User> ApplicationGetUserByEmailAsync(string username)
    //{
    //    return await db.Users.Where(u => u.Email == username).FirstOrDefaultAsync();
    //}
    //private async Task<User> ApplicationGetUserByMappedUsernameAsync(string username, int AuthenticationProviderId)
    //{
    //    var user = await (from u in db.Users
    //                      join uap in db.ApplicationAuthenticationProviderUserMappings on u.Id equals uap.UserId
    //                      where uap.Username == username
    //                      select u).FirstOrDefaultAsync();
    //    return user;
    //}
}

