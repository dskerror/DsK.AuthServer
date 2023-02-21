using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure;
public partial class SecurityService
{
    public async Task<APIResult<UserAuthenticationProviderDto>> UserAuthenticationProviderCreate(UserAuthenticationProviderCreateDto model)
    {
        APIResult<UserAuthenticationProviderDto> result = new APIResult<UserAuthenticationProviderDto>();

        int recordsCreated = 0;

        var record = new UserAuthenticationProvider();
        Mapper.Map(model, record);

        var checkDuplicateUsername = await db.UserAuthenticationProviders.FirstOrDefaultAsync(x => x.Username == model.Username && x.AuthenticationProviderId == model.AuthenticationProviderId);

        if (checkDuplicateUsername != null)
        {
            result.HasError = true;
            result.Message = "This username is already in use";
            return result;
        }

        db.UserAuthenticationProviders.Add(record);

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
    public async Task<APIResult<List<UserAuthenticationProvidersGridDto>>> UserAuthenticationProvidersGet(int userId)
    {

        var result = new APIResult<List<UserAuthenticationProvidersGridDto>>();
        var authenticationProviderList = await db.AuthenticationProviders.ToListAsync();


        var userAuthenticationProviderList = await (from uap in db.UserAuthenticationProviders
                                                    join ap in db.AuthenticationProviders on uap.AuthenticationProviderId equals ap.Id
                                                    where uap.UserId == userId
                                                    select new { uap.Id, uap.Username, AuthenticationProviderId = ap.Id }).ToListAsync();

        List<UserAuthenticationProvidersGridDto> userAuthenticationProvidersGridDtoList = new List<UserAuthenticationProvidersGridDto>();

        foreach (var item in authenticationProviderList)
        {
            userAuthenticationProvidersGridDtoList.Add(new UserAuthenticationProvidersGridDto
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
        var record = await db.UserAuthenticationProviders.FirstOrDefaultAsync(x => x.Id == model.Id);

        if (record != null)
            Mapper.Map(model, record);

        var checkDuplicateUsername =  await db.UserAuthenticationProviders.FirstOrDefaultAsync(x => x.Username == model.Username && x.AuthenticationProviderId == record.AuthenticationProviderId);

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
        var record = db.UserAuthenticationProviders.Attach(new UserAuthenticationProvider { Id = id });
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
