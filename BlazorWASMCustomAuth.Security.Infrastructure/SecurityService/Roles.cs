using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public APIResult RoleCreate(RoleCreateDto model)
        {
            APIResult result = new APIResult(model);
            int recordsCreated = 0;

            var record = new Role()
            {
                RoleName = model.RoleName,
                RoleDescription= model.RoleDescription
            };

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

            result.Result = record;
            if (recordsCreated == 1)
            {
                result.Message = "Record Created";
            }

            return result;
        }

        public APIResult RolesGet(int id = 0)
        {
            APIResult result = new APIResult(id);
            if (id == 0)
                result.Result = db.Roles.ToList();
            else
                result.Result = db.Roles.Where(x => x.Id == id).FirstOrDefault();

            return result;
        }

        public APIResult RoleUpdate(RoleUpdateDto model)
        {
            APIResult result = new APIResult(model);
            int recordsUpdated = 0;
            var record = db.Roles.FirstOrDefault(x => x.Id == model.Id);

            if (record != null)
            {
                record.RoleName = model.RoleName;
                record.RoleDescription = model.RoleDescription;
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

            result.Result = record;
            if (recordsUpdated == 1)
            {
                result.Message = "Record Updated";
            }

            return result;
        }
        public APIResult RoleDelete(int id)
        {
            APIResult result = new APIResult(id);
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

            return result;
        }
    }
}
