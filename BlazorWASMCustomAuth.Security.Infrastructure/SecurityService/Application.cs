using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public async Task<APIResult<ApplicationDto>> ApplicationCreate(ApplicationCreateDto model)
        {
            APIResult<ApplicationDto> result = new APIResult<ApplicationDto>();
            int recordsCreated = 0;

            var record = new Application();
            Mapper.Map(model, record);

            db.Applications.Add(record);

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
        public async Task<APIResult<List<ApplicationDto>>> ApplicationGet(int id, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            var result = new APIResult<List<ApplicationDto>>();

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
            List<Application> items;
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                count = await db.Applications
                    .Where(m => m.ApplicationName.Contains(searchString) || m.ApplicationDesc.Contains(searchString))
                    .CountAsync();

                items = await db.Applications.OrderBy(ordering)
                    .Where(m => m.ApplicationName.Contains(searchString) || m.ApplicationDesc.Contains(searchString))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            else if (id != 0)
            {
                count = await db.Applications
                    .Where(u => u.Id == id)
                    .CountAsync();

                items = await db.Applications.OrderBy(ordering)
                    .Where(u => u.Id == id)
                    .ToListAsync();
            }
            else
            {
                count = await db.Applications.CountAsync();

                items = await db.Applications.OrderBy(ordering)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            result.Paging.TotalItems = count;
            result.Result = Mapper.Map<List<Application>, List<ApplicationDto>>(items);
            return result;
        }
        public async Task<Application> ApplicationGet(int id)
        {
            var Application = await db.Applications.Where(u => u.Id == id).FirstOrDefaultAsync();
            return Application;
        }
        public async Task<APIResult<string>> ApplicationUpdate(ApplicationUpdateDto model)
        {
            APIResult<string> result = new APIResult<string>();

            int recordsUpdated = 0;
            var record = await db.Applications.FirstOrDefaultAsync(x => x.Id == model.Id);

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
        public async Task<APIResult<string>> ApplicationDelete(int id)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsDeleted = 0;

            var record = await db.Applications.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                db.Remove(record);
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
}
