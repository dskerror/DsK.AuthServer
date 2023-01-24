using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public APIResult RolePermissionCreate(RolePermission model)
        {
            APIResult result = new APIResult(model);
            int recordsCreated = 0;

            var record = new RolePermission()
            {
                RoleId = model.RoleId,
                PermissionId = model.PermissionId
            };

            db.RolePermissions.Add(record);

            try
            {
                recordsCreated = db.SaveChanges();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.InnerException.Message;
            }

            result.Result = record;
            if (recordsCreated == 1)
            {
                result.Message = "Record Created";
            }

            return result;
        }
        public APIResult RolePermissionsGet(int id = 0)
        {
            APIResult result = new APIResult(id);
            if (id == 0)
                result.Result = db.RolePermissions.ToList();
            else
                result.Result = db.RolePermissions.Where(x => x.RoleId == id).FirstOrDefault();

            return result;
        }

        public APIResult RolePermissionDelete(RolePermission model)
        {
            APIResult result = new APIResult(model);
            int recordsDeleted = 0;
            var record = db.RolePermissions.Attach(new RolePermission { RoleId = model.RoleId, PermissionId = model.PermissionId });
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
            
            return result;
        }
    }
}
