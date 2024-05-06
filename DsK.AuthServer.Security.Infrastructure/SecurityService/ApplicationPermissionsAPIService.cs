using DsK.AuthServer.Security.EntityFramework.Models;
using DsK.AuthServer.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Dynamic.Core;

namespace DsK.AuthServer.Security.Infrastructure;
public partial class SecurityService
{
    public async Task<APIResponse<ApplicationPermissionDto>> ApplicationPermissionCreate(ApplicationPermissionCreateDto model)
    {
        APIResponse<ApplicationPermissionDto> result = new APIResponse<ApplicationPermissionDto>();
        int recordsCreated = 0;

        var record = new ApplicationPermission();
        Mapper.Map(model, record);
        record.IsEnabled = true;
        db.ApplicationPermissions.Add(record);

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
    public async Task<APIResponse<List<ApplicationPermissionDto>>> ApplicationPermissionsGet(int ApplicationId, int Id, int PageNumber, int PageSize, string SearchString, string Orderby)
    {
        var result = new APIResponse<List<ApplicationPermissionDto>>();

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
        List<ApplicationPermission> items;
        if (!string.IsNullOrWhiteSpace(SearchString))
        {
            count = await db.ApplicationPermissions
                .Where(m => m.PermissionName.Contains(SearchString) || m.PermissionDescription.Contains(SearchString))
                .CountAsync();

            items = await db.ApplicationPermissions.OrderBy(ordering)
                .Where(m => m.PermissionName.Contains(SearchString) || m.PermissionDescription.Contains(SearchString))
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        else if (Id != 0)
        {
            count = await db.ApplicationPermissions
                .Where(u => u.Id == Id)
                .CountAsync();

            items = await db.ApplicationPermissions.OrderBy(ordering)
                .Where(u => u.Id == Id)
                .ToListAsync();
        }
        else
        {
            count = await db.ApplicationPermissions.Where(u => u.ApplicationId == ApplicationId).CountAsync();

            items = await db.ApplicationPermissions.Where(u => u.ApplicationId == ApplicationId).OrderBy(ordering)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        result.Paging.TotalItems = count;
        result.Result = Mapper.Map<List<ApplicationPermission>, List<ApplicationPermissionDto>>(items);
        return result;
    }
    public async Task<APIResponse<string>> ApplicationPermissionUpdate(ApplicationPermissionUpdateDto model)
    {
        APIResponse<string> result = new APIResponse<string>();
        int recordsUpdated = 0;
        var record = await db.ApplicationPermissions.FirstOrDefaultAsync(x => x.Id == model.Id);

        if (record != null)
        {
            Mapper.Map(model, record);
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
        {
            result.Result = recordsUpdated.ToString();
            result.Message = "Record Updated";
        }

        return result;
    }
    public async Task<APIResponse<string>> ApplicationPermissionDelete(int id)
    {
        APIResponse<string> result = new APIResponse<string>();
        int recordsDeleted = 0;
        var record = db.ApplicationPermissions.Attach(new ApplicationPermission { Id = id });
        record.State = EntityState.Deleted;
        try
        {
            recordsDeleted = await db.SaveChangesAsync();
            result.Result = recordsDeleted.ToString();
        }
        catch (Exception ex)
        {
            result.HasError = true;
            result.Message = ex.Message;
        }

        return result;
    }

    public async Task<APIResponse<string>> ApplicationPermissionIsEnabledToggle(int id)
    {
        APIResponse<string> result = new APIResponse<string>();
        int recordsUpdated = 0;

        var record = await db.ApplicationPermissions.FirstOrDefaultAsync(x => x.Id == id);

        try
        {
            record.IsEnabled = record.IsEnabled ? false : true;
            recordsUpdated = await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            result.HasError = true;
            result.Message = ex.Message;
        }

        result.Result = recordsUpdated.ToString();

        return result;
    }
}
