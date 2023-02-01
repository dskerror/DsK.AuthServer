using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public List<string> GetUserPermissions(string? username)
        {
            var permissionAllow = (from u in db.Users
                                   join up in db.UserPermissions on u.Id equals up.UserId
                                   join p in db.Permissions on up.PermissionId equals p.Id
                                   where u.Username == username && up.Allow == true
                                   select p.PermissionName).ToList();


            var permissionDeny = (from u in db.Users
                                  join up in db.UserPermissions on u.Id equals up.UserId
                                  join p in db.Permissions on up.PermissionId equals p.Id
                                  where u.Username == username && up.Allow == false
                                  select p.PermissionName).ToList();

            var RolePermissions = (from u in db.Users
                                   join ur in db.UserRoles on u.Id equals ur.UserId
                                   join r in db.Roles on ur.RoleId equals r.Id
                                   join rp in db.RolePermissions on r.Id equals rp.RoleId
                                   join p in db.Permissions on rp.PermissionId equals p.Id
                                   where u.Username == username
                                   select p.PermissionName).ToList();

            var finalList = permissionAllow.Concat(RolePermissions).Distinct().ToList();


            var setToRemove = new HashSet<string>(permissionDeny);
            finalList.RemoveAll(x => setToRemove.Contains(x));

            //return new APIResult(finalList);
            return finalList;
        }

        public APIResult<string> UserPermissionCreate(UserPermissionCreateDto model)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsCreated = 0;

            var record = new UserPermission();
            Mapper.Map(model, record);

            db.UserPermissions.Add(record);

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



        public APIResult<string> UserPermissionDelete(int id)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsDeleted = 0;
            var record = db.UserPermissions.Attach(new UserPermission { Id = id });
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
