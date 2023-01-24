using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Validations;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public APIResult UserRoleCreate(UserRoleCreateDto model)
        {
            APIResult result = new APIResult(model);
            int recordsCreated = 0;
            var record = new UserRole()
            {
                RoleId = model.RoleId,
                UserId = model.UserId
            }
            ;
            db.UserRoles.Add(record);

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

        public APIResult UserRolesGet(int userId)
        {
            APIResult result = new APIResult(userId);
            result.Result = db.UserRoles.Where(x => x.UserId == userId).Include(x => x.Role).ToList();

            return result;
        }

        public APIResult UserRoleDelete(int id)
        {
            APIResult result = new APIResult(id);
            int recordsDeleted = 0;
            var record = db.UserRoles.Attach(new UserRole { Id=id });
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
