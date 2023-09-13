using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure;
public partial class SecurityService
{
    public async Task<APIResult<UserAuthenticationProviderMappingDto>> UserAuthenticationProviderCreate(UserAuthenticationProviderCreateDto model)
    {
        APIResult<UserAuthenticationProviderMappingDto> result = new APIResult<UserAuthenticationProviderMappingDto>();

        int recordsCreated = 0;

        var record = new UserAuthenticationProviderMapping();
        Mapper.Map(model, record);

        var checkDuplicateUsername = await db.UserAuthenticationProviderMappings.FirstOrDefaultAsync(x => x.Username == model.Username && x.ApplicationAuthenticationProviderId == model.AuthenticationProviderId);

        if (checkDuplicateUsername != null)
        {
            result.HasError = true;
            result.Message = "This username is already in use";
            return result;
        }

        db.UserAuthenticationProviderMappings.Add(record);

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
    public async Task<APIResult<List<UserAuthenticationProviderMappingsGridDto>>> UserAuthenticationProvidersGet(int userId)
    {

        var result = new APIResult<List<UserAuthenticationProviderMappingsGridDto>>();
        var authenticationProviderList = await db.AuthenticationProviders.ToListAsync();


        var userAuthenticationProviderList = await (from uap in db.UserAuthenticationProviderMappings
                                                    join ap in db.ApplicationAuthenticationProviders on uap.ApplicationAuthenticationProviderId equals ap.Id
                                                    where uap.UserId == userId
                                                    select new { uap.Id, uap.Username, AuthenticationProviderId = ap.Id }).ToListAsync();

        List<UserAuthenticationProviderMappingsGridDto> userAuthenticationProvidersGridDtoList = new List<UserAuthenticationProviderMappingsGridDto>();

        foreach (var item in authenticationProviderList)
        {
            userAuthenticationProvidersGridDtoList.Add(new UserAuthenticationProviderMappingsGridDto
            {
                AuthenticationProviderId = item.Id,
                AuthenticationProviderName = item.AuthenticationProviderName,
                AuthenticationProviderType = item.AuthenticationProviderType
            });
        }

        foreach (var item in userAuthenticationProviderList)
        {
            var value = userAuthenticationProvidersGridDtoList.First(x => x.AuthenticationProviderId == item.AuthenticationProviderId);
            value.Username = item.Username;
            value.Id = item.Id;
        }

        result.Result = userAuthenticationProvidersGridDtoList;
        return result;
    }
    public async Task<APIResult<string>> UserAuthenticationProviderUpdate(UserAuthenticationProviderUpdateDto model)
    {
        APIResult<string> result = new APIResult<string>();
        int recordsUpdated = 0;
        var record = await db.UserAuthenticationProviderMappings.FirstOrDefaultAsync(x => x.Id == model.Id);

        if (record != null)
            Mapper.Map(model, record);

        var checkDuplicateUsername =  await db.UserAuthenticationProviderMappings.FirstOrDefaultAsync(x => x.Username == model.Username && x.ApplicationAuthenticationProviderId == record.ApplicationAuthenticationProviderId);

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
    public async Task<APIResult<string>> UserAuthenticationProviderDelete(int id)
    {
        APIResult<string> result = new APIResult<string>();
        int recordsDeleted = 0;
        var record = db.UserAuthenticationProviderMappings.Attach(new UserAuthenticationProviderMapping { Id = id });
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
