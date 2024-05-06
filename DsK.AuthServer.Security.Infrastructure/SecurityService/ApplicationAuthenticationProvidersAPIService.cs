using DsK.AuthServer.Security.EntityFramework.Models;
using DsK.AuthServer.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Data;
using DsK.Services;

namespace DsK.AuthServer.Security.Infrastructure;
public partial class SecurityService
{
    public async Task<APIResponse<ApplicationAuthenticationProviderDto>> ApplicationAuthenticationProvidersCreate(ApplicationAuthenticationProviderCreateDto model)
    {
        APIResponse<ApplicationAuthenticationProviderDto> result = new APIResponse<ApplicationAuthenticationProviderDto>();

        if (!await ApplicationAuthenticationProviderTypeNameIsValid(model.AuthenticationProviderType))
        {
            result.HasError = true;
            result.Message = "Application Authentication Provider Name Is Not Valid.";
            return result;
        }
        
        int recordsCreated = 0;

        var record = new ApplicationAuthenticationProvider();
        Mapper.Map(model, record);

        record.ApplicationAuthenticationProviderGuid = Guid.NewGuid();
        string key = $"{record.Id.ToString()} {record.Name}";
        record.Password = Encryption.AesOperation.EncryptString(key, record.Password);

        db.ApplicationAuthenticationProviders.Add(record);

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
    public async Task<APIResponse<List<ApplicationAuthenticationProviderDto>>> ApplicationAuthenticationProvidersGet(int ApplicationId, int Id, int PageNumber, int PageSize, string SearchString, string Orderby)
    {
        var result = new APIResponse<List<ApplicationAuthenticationProviderDto>>();

        string ordering = "Id";
        if (!string.IsNullOrWhiteSpace(Orderby))
        {
            string[] OrderBy = Orderby.Split(',');
            ordering = string.Join(",", OrderBy);
        }
        result.Paging.CurrentPage = PageNumber;
        PageNumber = PageNumber == 0 ? 1 : PageNumber;
        PageSize = PageSize == 0 ? 10 : PageSize;
        int count = 0;
        List<ApplicationAuthenticationProvider> items;
        if (!string.IsNullOrWhiteSpace(SearchString))
        {
            count = await db.ApplicationAuthenticationProviders
                .Where(m => m.Name.Contains(SearchString))
                .CountAsync();

            items = await db.ApplicationAuthenticationProviders.OrderBy(ordering)
                .Where(m => m.Name.Contains(SearchString))
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        else if (Id != 0)
        {
            count = await db.ApplicationAuthenticationProviders
                .Where(u => u.Id == Id)
                .CountAsync();

            items = await db.ApplicationAuthenticationProviders.OrderBy(ordering)
                .Where(u => u.Id == Id)
                .ToListAsync();
        }
        else if (PageNumber == -1)
        {
            count = await db.ApplicationAuthenticationProviders.Where(u => u.ApplicationId == ApplicationId).CountAsync();
            items = await db.ApplicationAuthenticationProviders.Where(u => u.ApplicationId == ApplicationId).ToListAsync();
        }
        else
        {
            count = await db.ApplicationAuthenticationProviders.Where(u => u.ApplicationId == ApplicationId).CountAsync();
            items = await db.ApplicationAuthenticationProviders.Where(u => u.ApplicationId == ApplicationId).OrderBy(ordering)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        result.Paging.TotalItems = count;
        result.Result = Mapper.Map<List<ApplicationAuthenticationProvider>, List<ApplicationAuthenticationProviderDto>>(items);
        return result;
    }
    public async Task<ApplicationAuthenticationProvider> ApplicationAuthenticationProviderGet(Guid ApplicationAuthenticationProviderGUID)
    {
        ApplicationAuthenticationProvider applicationAuthenticationProvider;

        if (ApplicationAuthenticationProviderGUID == Guid.Empty)
            applicationAuthenticationProvider = await db.ApplicationAuthenticationProviders.Where(u => u.Id == 1).Include(x => x.Application).FirstOrDefaultAsync();
        else
            applicationAuthenticationProvider = await db.ApplicationAuthenticationProviders.Where(u => u.ApplicationAuthenticationProviderGuid == ApplicationAuthenticationProviderGUID).Include(x => x.Application).FirstOrDefaultAsync();

        if (applicationAuthenticationProvider is null)
            return null;

        string key = $"{applicationAuthenticationProvider.Id.ToString()} {applicationAuthenticationProvider.Name}";
        applicationAuthenticationProvider.Password = Encryption.AesOperation.DecryptString(key, applicationAuthenticationProvider.Password);

        return applicationAuthenticationProvider;
    }

    public APIResponse<string> ValidateApplicationAuthenticationProvider(ApplicationAuthenticationProvider applicationAuthenticationProvider)
    {
        var r = new APIResponse<string>();
        if (applicationAuthenticationProvider is null)
        {
            r.HasError = true;
            r.Message = "This Application Authentication Provider Doesn't Exists.";
            return r;
        }
        if (!applicationAuthenticationProvider.IsEnabled)
        {
            r.HasError = true;
            r.Message = "This Application Authentication Provider Is Disabled";
            return r;
        }

        return r;
    }
    public async Task<APIResponse<string>> ApplicationAuthenticationProvidersUpdate(ApplicationAuthenticationProviderDto model)
    {
        APIResponse<string> result = new APIResponse<string>();

        int recordsUpdated = 0;
        var record = await db.ApplicationAuthenticationProviders.FirstOrDefaultAsync(x => x.Id == model.Id);

        if (record != null)
            Mapper.Map(model, record);

        string key = $"{record.Id.ToString()} {record.Name}";
        record.Password = Encryption.AesOperation.EncryptString(key, record.Password);

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
    public async Task<APIResponse<string>> ApplicationAuthenticationProvidersDelete(int id)
    {
        APIResponse<string> result = new APIResponse<string>();
        int recordsDeleted = 0;
        var record = await db.ApplicationAuthenticationProviders.FirstOrDefaultAsync(x => x.Id == id);

        try
        {
            if (record.AuthenticationProviderType == "Local")
            {
                result.HasError = true;
                result.Message = "Local Authentication Provider can't be deleted";
            }
            else
            {
                //Delete All User Mapping First
                var mappings = await db.ApplicationAuthenticationProviderUserMappings.FirstOrDefaultAsync(x => x.ApplicationAuthenticationProviderId == record.Id);
                db.Remove(mappings);
                await db.SaveChangesAsync();

                db.Remove(record);
                recordsDeleted = await db.SaveChangesAsync();
            }

        }
        catch (Exception ex)
        {
            result.HasError = true;
            result.Message = $"Message : {ex.Message}{Environment.NewLine}Inner Exception : {ex.InnerException}";
        }

        result.Result = recordsDeleted.ToString();
        return result;
    }
    public async Task<APIResponse<string>> ApplicationAuthenticationProviderIsEnabledToggle(int id)
    {
        APIResponse<string> result = new APIResponse<string>();
        int recordsUpdated = 0;

        var record = await db.ApplicationAuthenticationProviders.FirstOrDefaultAsync(x => x.Id == id);

        try
        {
            if (record.AuthenticationProviderType == "Local")
            {
                result.HasError = true;
                result.Message = "Local Authentication Provider can't be disabled";
            }
            else
            {
                record.IsEnabled = record.IsEnabled ? false : true;
                recordsUpdated = await db.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            result.HasError = true;
            result.Message = ex.Message;
        }

        result.Result = recordsUpdated.ToString();

        return result;
    }
    public async Task<APIResponse<ApplicationAuthenticationProviderValidateDto>> ValidateApplicationAuthenticationProviderGuid(string applicationAuthenticationProviderGuid)
    {
        var result = new APIResponse<ApplicationAuthenticationProviderValidateDto>();

        try
        {
            var providerDto = new ApplicationAuthenticationProviderValidateDto();
            var provider = await db.ApplicationAuthenticationProviders.Where(m => m.ApplicationAuthenticationProviderGuid == Guid.Parse(applicationAuthenticationProviderGuid)).FirstOrDefaultAsync();
            Mapper.Map(provider, providerDto);
            result.Result = providerDto;
        }
        catch (Exception)
        {
            result.Result = null;
        }

        return result;
    }
}
