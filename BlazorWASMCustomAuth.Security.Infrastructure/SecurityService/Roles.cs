using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure;
public partial class SecurityService
{
    public APIResult<RoleDto> RoleCreate(RoleCreateDto model)
    {
        APIResult<RoleDto> result = new APIResult<RoleDto>();
        int recordsCreated = 0;

        var record = new Role();
        Mapper.Map(model, record);

        db.Roles.Add(record);

        try
        {
            recordsCreated = db.SaveChanges();
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
    public APIResult<List<RoleDto>> RolesGet(int id, int pageNumber, int pageSize, string searchString, string orderBy)
    {
        var result = new APIResult<List<RoleDto>>();

        string ordering = "Id";
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            string[] OrderBy = orderBy.Split(',');
            ordering = string.Join(",", OrderBy);
        }
        result.Paging.CurrentPage = pageNumber;
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 10 : pageSize;
        int count = 0;
        List<Role> items;
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            count = db.Roles
                .Where(m => m.RoleName.Contains(searchString) || m.RoleDescription.Contains(searchString))
                .Count();

            items = db.Roles.OrderBy(ordering)
                .Where(m => m.RoleName.Contains(searchString) || m.RoleDescription.Contains(searchString))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        else if (id != 0)
        {
            count = db.Roles
                .Where(u => u.Id == id)
                .Count();

            items = db.Roles.OrderBy(ordering)
                .Where(u => u.Id == id)
                .ToList();
        }
        else
        {
            count = db.Roles.Count();

            items = db.Roles.OrderBy(ordering)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        result.Paging.TotalItems = count;
        result.Result = Mapper.Map<List<Role>, List<RoleDto>>(items);
        return result;
    }
    public APIResult<string> RoleUpdate(RoleUpdateDto model)
    {
        APIResult<string> result = new APIResult<string>();
        int recordsUpdated = 0;
        var record = db.Roles.FirstOrDefault(x => x.Id == model.Id);

        if (record != null)
            Mapper.Map(model, record);

        try
        {
            recordsUpdated = db.SaveChanges();
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
    public APIResult<string> RoleDelete(int id)
    {
        APIResult<string> result = new APIResult<string>();
        int recordsDeleted = 0;
        var record = db.Roles.Attach(new Role { Id = id });
        record.State = EntityState.Deleted;
        try
        {
            recordsDeleted = db.SaveChanges();            
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
