using DsK.AuthServer.Security.EntityFramework.Models;
using DsK.AuthServer.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static DsK.AuthServer.Security.Shared.Access;

namespace DsK.AuthServer.Security.Infrastructure;
public partial class SecurityService
{
    public async Task<APIResult<string>> UserRoleChange(UserRoleChangeDto model)
    {
        APIResult<string> result = new APIResult<string>();
        int recordsModifiedCount = 0;
        var record = new UserRole();
        Mapper.Map(model, record);

        if (model.IsEnabled)
        {
            db.UserRoles.Add(record);
            try
            {
                recordsModifiedCount = await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.InnerException.Message;
            }
            if (recordsModifiedCount == 1)
            {
                result.Result = recordsModifiedCount.ToString();
                result.Message = "Record Created";
            }
        }
        else
        {
            var recordFind = await db.UserRoles.Where(x => x.UserId == model.UserId && x.RoleId == model.RoleId).FirstOrDefaultAsync();
            var recordToDelete = db.UserRoles.Attach(recordFind);
            recordToDelete.State = EntityState.Deleted;

            try
            {
                recordsModifiedCount = await db.SaveChangesAsync();
                result.Result = recordsModifiedCount.ToString();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }
        }
        return result;
    }

    public async Task<APIResult<List<UserRoleGridDto>>> UserRolesGet(int userId)
    {
        APIResult<List<UserRoleGridDto>> result = new APIResult<List<UserRoleGridDto>>();
        var applicationRoles = await db.ApplicationRoles
            .Include(x => x.Application)
            .ThenInclude(x => x.ApplicationUsers)
            .Where(w => w.Application.ApplicationUsers.Any(a => a.UserId == userId))
            .ToListAsync();

        //var applicationRoles = await (from ar in db.ApplicationRoles
        //                              join a in db.Applications on ar.ApplicationId equals a.Id
        //                              join au in db.ApplicationUsers on a.Id equals au.ApplicationId
        //                              where au.UserId == userId
        //                              select ar, a
        //                              ).ToListAsync();

        var userRoles = await (from u in db.Users
                               join ur in db.UserRoles on u.Id equals ur.UserId
                               join r in db.ApplicationRoles on ur.RoleId equals r.Id
                               join a in db.Applications on r.ApplicationId equals a.Id
                               where u.Id == userId
                               select new { r.RoleName, a.Id }).ToListAsync();

        //var roleGrid = Mapper.Map<List<ApplicationRole>, List<UserRoleGridDto>>(roleList);

        List<UserRoleGridDto> userRoleGridDtos = new List<UserRoleGridDto>();

        foreach (var role in applicationRoles)
        {
            var value = new UserRoleGridDto()
            {
                Id = role.Id,
                ApplicationName = role.Application.ApplicationName,
                RoleName = role.RoleName,
                RoleDescription = role.RoleDescription,
            };

            var lookupInUserRole = userRoles.Where(x => x.RoleName == role.RoleName && x.Id == role.ApplicationId).FirstOrDefault();
            if (lookupInUserRole != null) { value.IsEnabled = true; }

            userRoleGridDtos.Add(value);
        }

        result.Result = userRoleGridDtos;
        return result;
    }
}
