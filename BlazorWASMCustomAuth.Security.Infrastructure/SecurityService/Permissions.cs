using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public async Task<APIResult<string>> PermissionCreate(PermissionCreateDto model)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsCreated = 0;

            var record = new ApplicationPermission();
            Mapper.Map(model, record);

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
                result.Result = recordsCreated.ToString();
                result.Message = "Record Created";
            }

            return result;
        }
        public async Task<APIResult<List<ApplicationPermissionDto>>> PermissionsGet (int id = 0)
        {
            APIResult<List<ApplicationPermissionDto>> result = new APIResult<List<ApplicationPermissionDto>>();
            if (id == 0)
            {
                var items = await db.ApplicationPermissions.ToListAsync();
                result.Result = Mapper.Map<List<ApplicationPermission>, List<ApplicationPermissionDto>>(items);
            }
            else
            {
                var items = await db.ApplicationPermissions.Where(x => x.Id == id).ToListAsync();
                result.Result = Mapper.Map<List<ApplicationPermission>, List<ApplicationPermissionDto>>(items);
            }

            return result;
        }
        public async Task<APIResult<string>> PermissionUpdate(PermissionUpdateDto model)
        {
            APIResult<string> result = new APIResult<string>();
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
        public async Task<APIResult<string>> PermissionDelete(int id)
        {
            APIResult<string> result = new APIResult<string>();
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
    }
}
