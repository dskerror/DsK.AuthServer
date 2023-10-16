using DsK.AuthServer.Security.EntityFramework.Models;
using DsK.AuthServer.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Data;

namespace DsK.AuthServer.Security.Infrastructure;
public partial class SecurityService
{
    public async Task<APIResult<ApplicationRoleDto>> ApplicationRoleCreate(ApplicationRoleCreateDto model)
    {
        APIResult<ApplicationRoleDto> result = new APIResult<ApplicationRoleDto>();
        int recordsCreated = 0;

        var record = new ApplicationRole();
        Mapper.Map(model, record);

        db.ApplicationRoles.Add(record);

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
    public async Task<APIResult<List<ApplicationRoleDto>>> ApplicationRolesGet(int ApplicationId, int Id, int PageNumber, int PageSize, string SearchString, string Orderby)
    {
        var result = new APIResult<List<ApplicationRoleDto>>();

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
        List<ApplicationRole> items;
        if (!string.IsNullOrWhiteSpace(SearchString))
        {
            count = await db.ApplicationRoles
                .Where(m => m.RoleName.Contains(SearchString) || m.RoleDescription.Contains(SearchString))
                .CountAsync();

            items = await db.ApplicationRoles.OrderBy(ordering)
                .Where(m => m.RoleName.Contains(SearchString) || m.RoleDescription.Contains(SearchString))
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        else if (Id != 0)
        {
            count = await db.ApplicationRoles
                .Where(u => u.Id == Id)
                .CountAsync();

            items = await db.ApplicationRoles.OrderBy(ordering)
                .Where(u => u.Id == Id)
                .ToListAsync();
        }
        else if (PageNumber == -1)
        {
            if (ApplicationId > 0)
            {
                count = await db.ApplicationRoles.Where(u => u.ApplicationId == ApplicationId).CountAsync();
                items = await db.ApplicationRoles.Where(u => u.ApplicationId == ApplicationId).ToListAsync();
            }
            else
            {
                count = await db.ApplicationRoles.CountAsync();
                items = await db.ApplicationRoles.OrderBy(ordering).ToListAsync();
            }
        }
        else
        {
            count = await db.ApplicationRoles.Where(u => u.ApplicationId == ApplicationId).CountAsync();

            items = await db.ApplicationRoles.Where(u => u.ApplicationId == ApplicationId).OrderBy(ordering)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        result.Paging.TotalItems = count;
        result.Result = Mapper.Map<List<ApplicationRole>, List<ApplicationRoleDto>>(items);
        return result;
    }
    public async Task<APIResult<string>> ApplicationRoleUpdate(ApplicationRoleUpdateDto model)
    {
        APIResult<string> result = new APIResult<string>();
        int recordsUpdated = 0;
        var record = await db.ApplicationRoles.FirstOrDefaultAsync(x => x.Id == model.Id);

        if (record != null)
            Mapper.Map(model, record);

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
    public async Task<APIResult<string>> ApplicationRoleDelete(int id)
    {
        APIResult<string> result = new APIResult<string>();
        int recordsDeleted = 0;
        var record = db.ApplicationRoles.Attach(new ApplicationRole { Id = id });
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
    public async Task<APIResult<string>> ApplicationRoleIsEnabledToggle(int id)
    {
        APIResult<string> result = new APIResult<string>();
        int recordsUpdated = 0;

        var record = await db.ApplicationRoles.FirstOrDefaultAsync(x => x.Id == id);

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
