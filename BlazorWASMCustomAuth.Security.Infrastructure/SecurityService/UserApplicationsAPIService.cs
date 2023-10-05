using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {       
        public async Task<APIResult<List<ApplicationUserDto>>> UserApplicationsGet(int id, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            var result = new APIResult<List<ApplicationUserDto>>();

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
            List<ApplicationUser> items;
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                count = await db.ApplicationUsers
                    .Include(x => x.Application)
                    .Where(m => m.Id == id && m.Application.ApplicationName.Contains(searchString) || m.Application.ApplicationDesc.Contains(searchString))
                    .CountAsync();

                items = await db.ApplicationUsers
                    .Include(x => x.Application)
                    .OrderBy(ordering)                    
                    .Where(m => m.Id == id && m.Application.ApplicationName.Contains(searchString) || m.Application.ApplicationDesc.Contains(searchString))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            else
            {
                count = await db.ApplicationUsers
                    .Include(x => x.Application)
                    .Where(m => m.Id == id && m.Application.ApplicationName.Contains(searchString) || m.Application.ApplicationDesc.Contains(searchString))
                    .CountAsync();

                items = await db.ApplicationUsers
                    .Include(x => x.Application)
                    .OrderBy(ordering)
                    .Where(m => m.Id == id && m.Application.ApplicationName.Contains(searchString) || m.Application.ApplicationDesc.Contains(searchString))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            result.Paging.TotalItems = count;
            result.Result = Mapper.Map<List<ApplicationUser>, List<ApplicationUserDto>>(items);
            return result;
        }        
    }
}
