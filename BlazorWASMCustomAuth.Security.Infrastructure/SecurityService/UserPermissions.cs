using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {  
        public List<string> GetUserPermissions(string? username)
        {
            List<string> permissions = new List<string>();

            var userPermissionsDt = dm.ExecDataTableSP("sp_UserPermissions", "Username", username ?? "");

            foreach (DataRow permission in userPermissionsDt.Rows)
            {
                permissions.Add(permission[0].ToString() ?? "");
            }
            return permissions;
        }

        public APIResult GetUserPermissionsNew(string? username)
        {
            var x = db.Users.Where(x => x.Username == username).Include(x => x.UserRoles).ThenInclude(x => x.Role).ThenInclude(x => x.RolePermissions).ToList();
            return new APIResult(x);
        }

        public APIResult UserPermissionCreate(UserPermission model)
        {
            APIResult result = new APIResult(model);
            int recordsCreated = 0;
          
            db.UserPermissions.Add(model);

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
                result.Message = "Record Created";
            }

            return result;
        }

    

        public APIResult UserPermissionDelete(int id)
        {
            APIResult result = new APIResult(id);
            int recordsDeleted = 0;
            var record = db.UserPermissions.Attach(new UserPermission { Id = id });
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
