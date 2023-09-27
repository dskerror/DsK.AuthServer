using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure;
public partial class SecurityService
{
    public async Task<APIResult<ApplicationAuthenticationProviderUserMappingDto>> ApplicationAuthenticationProviderUserMappingCreate(ApplicationAuthenticationProviderUserMappingCreateDto model)
    {
        APIResult<ApplicationAuthenticationProviderUserMappingDto> result = new APIResult<ApplicationAuthenticationProviderUserMappingDto>();

        int recordsCreated = 0;

        var record = new ApplicationAuthenticationProviderUserMapping();
        Mapper.Map(model, record);

        var checkDuplicateUsername = await db.ApplicationAuthenticationProviderUserMappings.FirstOrDefaultAsync(x => x.Username == model.Username);

        if (checkDuplicateUsername != null)
        {
            result.HasError = true;
            result.Message = "This username is already in use";
            return result;
        }

        db.ApplicationAuthenticationProviderUserMappings.Add(record);

        try
        {
            recordsCreated = await db.SaveChangesAsync();
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
    public async Task<APIResult<List<ApplicationAuthenticationProviderUserMappingsGridDto>>> ApplicationAuthenticationProviderUserMappingsGet(int userId)
    {

        var result = new APIResult<List<ApplicationAuthenticationProviderUserMappingsGridDto>>();
        var authenticationProviderList = await db.ApplicationAuthenticationProviders.ToListAsync();


        var applicationAuthenticationProviderUserMappingsList = await (from uap in db.ApplicationAuthenticationProviderUserMappings
                                                        //join ap in db.ApplicationAuthenticationProviders on uap.ApplicationAuthenticationProviderId equals ap.Id
                                                    where uap.UserId == userId
                                                    select new { uap.Id, uap.Username }).ToListAsync();

        List<ApplicationAuthenticationProviderUserMappingsGridDto> applicationAuthenticationProviderUserMappingGridDtoList = new List<ApplicationAuthenticationProviderUserMappingsGridDto>();

        foreach (var item in authenticationProviderList)
        {
            applicationAuthenticationProviderUserMappingGridDtoList.Add(new ApplicationAuthenticationProviderUserMappingsGridDto
            {   
                Name = item.Name,
                AuthenticationProviderType = item.AuthenticationProviderType
            });
        }

        foreach (var item in applicationAuthenticationProviderUserMappingsList)
        {
            var value = applicationAuthenticationProviderUserMappingGridDtoList.First();
            value.Username = item.Username;
            value.Id = item.Id;
        }

        result.Result = applicationAuthenticationProviderUserMappingGridDtoList;
        return result;
    }
    public async Task<APIResult<string>> ApplicationAuthenticationProviderUserMappingUpdate(ApplicationAuthenticationProviderUserMappingUpdateDto model)
    {
        APIResult<string> result = new APIResult<string>();
        int recordsUpdated = 0;
        var record = await db.ApplicationAuthenticationProviderUserMappings.FirstOrDefaultAsync(x => x.Id == model.Id);

        if (record != null)
            Mapper.Map(model, record);

        var checkDuplicateUsername =  await db.ApplicationAuthenticationProviderUserMappings.FirstOrDefaultAsync(x => x.Username == model.Username);

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
    public async Task<APIResult<string>> ApplicationAuthenticationProviderUserMappingDelete(int id)
    {
        APIResult<string> result = new APIResult<string>();
        int recordsDeleted = 0;
        var record = db.ApplicationAuthenticationProviderUserMappings.Attach(new ApplicationAuthenticationProviderUserMapping { Id = id });
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
}
