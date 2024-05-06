using DsK.AuthServer.Security.EntityFramework.Models;
using DsK.AuthServer.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Data;

namespace DsK.AuthServer.Security.Infrastructure;
public partial class SecurityService
{
    public async Task<bool> ApplicationAuthenticationProviderUserMappingIsEnabledToggleDto(ApplicationAuthenticationProviderUserMappingIsEnabledToggleDto model)
    {
        try
        {
            var userMapping = await db.ApplicationAuthenticationProviderUserMappings
                .Where(x => x.ApplicationUserId == model.ApplicationUserId && x.ApplicationAuthenticationProviderId == model.ApplicationAuthenticationProviderId)
                .SingleOrDefaultAsync();

            if (userMapping == null)
            {
                var applicationUser = await db.ApplicationUsers.Include(x => x.User).Where(x => x.Id == model.ApplicationUserId).SingleOrDefaultAsync();
                var mapping = new ApplicationAuthenticationProviderUserMapping()
                {
                    ApplicationAuthenticationProviderId = model.ApplicationAuthenticationProviderId,
                    ApplicationUserId = model.ApplicationUserId,
                    IsEnabled = true,
                    Username = applicationUser.User.Email
                };

                db.ApplicationAuthenticationProviderUserMappings.Add(mapping);
            }
            else
                userMapping.IsEnabled = userMapping.IsEnabled ? false : true;

            await db.SaveChangesAsync();

        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
    public async Task<APIResponse<List<ApplicationAuthenticationProviderUserMappingsGridDto>>> ApplicationAuthenticationProviderUserMappingsGet(int applicationId, int applicationUserId)
    {
        var result = new APIResponse<List<ApplicationAuthenticationProviderUserMappingsGridDto>>();
        List<ApplicationAuthenticationProviderUserMappingsGridDto> applicationAuthenticationProviderUserMappingGridDtoList = new List<ApplicationAuthenticationProviderUserMappingsGridDto>();

        var authenticationProviderList =
            await db.ApplicationAuthenticationProviders.Where(x => x.ApplicationId == applicationId).ToListAsync();
        var applicationUser = await db.ApplicationUsers.Include(x => x.User).Where(x => x.UserId == applicationUserId && x.ApplicationId == applicationId).SingleOrDefaultAsync();
        var applicationAuthenticationProviderUserMappingsList =
            await db.ApplicationAuthenticationProviderUserMappings.Where(x => x.ApplicationUserId == applicationUser.Id).ToListAsync();
        

        foreach (var item in authenticationProviderList)
        {
            applicationAuthenticationProviderUserMappingGridDtoList.Add(new ApplicationAuthenticationProviderUserMappingsGridDto
            {
                ApplicationId = applicationId,
                UserId = applicationUserId,
                Email = applicationUser.User.Email,
                ApplicationAuthenticationProviderId = item.Id,
                AuthenticationProviderName = item.Name,
                AuthenticationProviderType = item.AuthenticationProviderType
            });
        }

        foreach (var item in applicationAuthenticationProviderUserMappingGridDtoList)
        {
            var value = applicationAuthenticationProviderUserMappingsList.Where(x => x.ApplicationUserId == applicationUser.Id && x.ApplicationAuthenticationProviderId == item.ApplicationAuthenticationProviderId).SingleOrDefault();
            if (value != null)
            {
                item.Id = value.Id;
                item.Username = value.Username;
                item.IsEnabled = value.IsEnabled;
            }
        }

        result.Result = applicationAuthenticationProviderUserMappingGridDtoList;
        return result;
    }
    public async Task<APIResponse<string>> ApplicationAuthenticationProviderUserMappingUpdate(ApplicationAuthenticationProviderUserMappingUpdateDto model)
    {
        APIResponse<string> result = new APIResponse<string>();
        int recordsUpdated = 0;
        var record = await db.ApplicationAuthenticationProviderUserMappings.FirstOrDefaultAsync(x => x.Id == model.Id);

        if (record != null)
            Mapper.Map(model, record);
        else
        {
            result.HasError = true;
            result.Message = "Id not found";
            return result;
        }

        var checkDuplicateUsername = await db.ApplicationAuthenticationProviderUserMappings
            .FirstOrDefaultAsync(x => x.Username == model.Username
                //&& x.ApplicationAuthenticationProviderId == record.ApplicationAuthenticationProviderId 
                && x.ApplicationUserId != record.ApplicationUserId);

        if (checkDuplicateUsername != null)
        {
            result.HasError = true;
            result.Message = "This username is already in use";
            return result;
        }

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
    //public async Task<APIResult<string>> ApplicationAuthenticationProviderUserMappingDelete(int id)
    //{
    //    APIResult<string> result = new APIResult<string>();
    //    int recordsDeleted = 0;
    //    var record = db.ApplicationAuthenticationProviderUserMappings.Attach(new ApplicationAuthenticationProviderUserMapping { Id = id });
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
}
