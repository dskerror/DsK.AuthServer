using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public APIResult<string> PermissionCreate(PermissionCreateDto model)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsCreated = 0;

            var record = new Permission();
            Mapper.Map(model, record);

            db.Permissions.Add(record);

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
                result.Result = recordsCreated.ToString();
                result.Message = "Record Created";
            }

            return result;
        }

        public APIResult<List<PermissionDto>> PermissionsGet (int id = 0)
        {
            APIResult<List<PermissionDto>> result = new APIResult<List<PermissionDto>>();
            if (id == 0)
            {
                var items = db.Permissions.ToList();
                result.Result = Mapper.Map<List<Permission>, List<PermissionDto>>(items);
            }
            else
            {
                var items = db.Permissions.Where(x => x.Id == id).ToList();
                result.Result = Mapper.Map<List<Permission>, List<PermissionDto>>(items);
            }

            return result;
        }
        public APIResult<string> PermissionUpdate(PermissionUpdateDto model)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsUpdated = 0;
            var record = db.Permissions.FirstOrDefault(x => x.Id == model.Id);

            if (record != null)
            {
                Mapper.Map(model, record);                
            }

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
            {
                result.Result = recordsUpdated.ToString();
                result.Message = "Record Updated";
            }

            return result;
        }
        public APIResult<string> PermissionDelete(int id)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsDeleted = 0;
            var record = db.Permissions.Attach(new Permission { Id = id });
            record.State = EntityState.Deleted;
            try
            {
                recordsDeleted = db.SaveChanges();
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
