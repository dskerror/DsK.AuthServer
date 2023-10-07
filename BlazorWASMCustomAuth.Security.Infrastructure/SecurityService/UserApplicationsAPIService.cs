using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure;

public partial class SecurityService
{
    public async Task<APIResult<List<UserApplicationGridDto>>> UserApplicationsGet(int userId)
    {
        APIResult<List<UserApplicationGridDto>> result = new APIResult<List<UserApplicationGridDto>>();

        var applications = await db.Applications.ToListAsync();

        var userApplications = await (from au in db.ApplicationUsers
                               join a in db.Applications on au.ApplicationId equals a.Id
                               where au.UserId == userId
                               select new { a.ApplicationName, au.UserId, au.ApplicationId }).ToListAsync();

        List<UserApplicationGridDto> userApplicationGridDto = new List<UserApplicationGridDto>();

        foreach (var application in applications)
        {
            var value = new UserApplicationGridDto()
            {   
                ApplicationName = application.ApplicationName,
                ApplicationId = application.Id,
                UserId = userId,                
            };

            var lookupInuserApplications = userApplications.Where(x => x.UserId == userId && x.ApplicationId == application.Id).FirstOrDefault();
            if (lookupInuserApplications != null) { value.IsEnabled = true; }

            userApplicationGridDto.Add(value);
        }

        result.Result = userApplicationGridDto;
        return result;
    }
    public async Task<APIResult<string>> UserApplicationChange(ApplicationUserChangeDto model)
    {
        APIResult<string> result = new APIResult<string>();

        if(model.UserId == 1 & model.ApplicationId == 1)
        {
            result.HasError = true;
            result.Message = "Admin can't be disabled from main app";
            return result;
        }

        int recordsModifiedCount = 0;
        var record = new ApplicationUser();
        Mapper.Map(model, record);

        if (model.IsEnabled)
        {
            db.ApplicationUsers.Add(record);
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
            var recordFind = await db.ApplicationUsers.Where(x => x.ApplicationId == model.ApplicationId && x.UserId == model.UserId).FirstOrDefaultAsync();
            var recordToDelete = db.ApplicationUsers.Attach(recordFind);
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
}
