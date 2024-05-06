using DsK.AuthServer.Security.EntityFramework.Models;
using DsK.AuthServer.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace DsK.AuthServer.Security.Infrastructure;

public partial class SecurityService
{
    public async Task<APIResponse<List<UserApplicationGridDto>>> UserApplicationsGet(int userId)
    {
        APIResponse<List<UserApplicationGridDto>> result = new APIResponse<List<UserApplicationGridDto>>();

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
    public async Task<APIResponse<string>> UserApplicationChange(ApplicationUserChangeDto model)
    {
        APIResponse<string> result = new APIResponse<string>();

        if (model.UserId == 1 & model.ApplicationId == 1)
        {
            result.HasError = true;
            result.Message = "Admin can't be disabled from main app";
            return result;
        }

        int recordsModifiedCount = 0;
        var record = new ApplicationUser();
        Mapper.Map(model, record);

        var applicationUser = db.ApplicationUsers.Where(x => x.ApplicationId == model.ApplicationId && x.UserId == model.UserId).SingleOrDefault();

        if (applicationUser == null)
        {
            var appAuthProvider = await db.ApplicationAuthenticationProviders.Where(x => x.AuthenticationProviderType == "Local" && x.ApplicationId == model.ApplicationId).SingleOrDefaultAsync();
            var user = await db.Users.FindAsync(model.UserId);

            using (var transaction = db.Database.BeginTransaction())
            {

                try
                {
                    db.ApplicationUsers.Add(record);
                    await db.SaveChangesAsync();

                    var mapping = new ApplicationAuthenticationProviderUserMapping()
                    {
                        ApplicationAuthenticationProviderId = appAuthProvider.Id,
                        ApplicationUser = record,
                        IsEnabled = true,
                        Username = user.Email
                    };

                    db.ApplicationAuthenticationProviderUserMappings.Add(mapping);
                    recordsModifiedCount = await db.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result.HasError = true;
                    result.Message = ex.InnerException.Message;
                    return result;
                }

                result.Result = recordsModifiedCount.ToString();
                result.Message = "Record Created";

            }
        }
        else
        {
            applicationUser.IsEnabled = applicationUser.IsEnabled ? false : true;
            recordsModifiedCount = await db.SaveChangesAsync();
            result.Result = recordsModifiedCount.ToString();
            result.Message = "Record Modified";
        }

        return result;
    }
}
