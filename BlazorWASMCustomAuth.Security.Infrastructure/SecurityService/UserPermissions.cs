using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public async Task<APIResult<string>> UserPermissionChange(UserPermissionChangeDto model)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsModifiedCount = 0;

            var record = new UserPermission();
            Mapper.Map(model, record);

            db.UserPermissions.Add(record);

            //try
            //{
            //    recordsCreated = await db.SaveChangesAsync();
            //}
            //catch (Exception ex)
            //{
            //    result.HasError = true;
            //    result.Message = ex.InnerException.Message;
            //}

            //if (recordsCreated == 1)
            //{
            //    result.Result = recordsCreated.ToString();
            //    result.Message = "Record Created";
            //}

            return result;
        }



        public async Task< APIResult<string>> UserPermissionDelete(int id)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsDeleted = 0;
            var record = db.UserPermissions.Attach(new UserPermission { Id = id });
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

        public async Task<List<string>> GetUserPermissions(int userId)
        {
            var permissionAllow = await (from u in db.Users
                                         join up in db.UserPermissions on u.Id equals up.UserId
                                         join p in db.Permissions on up.PermissionId equals p.Id
                                         where u.Id == userId && up.Allow == true
                                         select p.PermissionName).ToListAsync();


            var permissionDeny = await (from u in db.Users
                                        join up in db.UserPermissions on u.Id equals up.UserId
                                        join p in db.Permissions on up.PermissionId equals p.Id
                                        where u.Id == userId && up.Allow == false
                                        select p.PermissionName).ToListAsync();

            var RolePermissions = await (from u in db.Users
                                         join ur in db.UserRoles on u.Id equals ur.UserId
                                         join r in db.Roles on ur.RoleId equals r.Id
                                         join rp in db.RolePermissions on r.Id equals rp.RoleId
                                         join p in db.Permissions on rp.PermissionId equals p.Id
                                         where u.Id == userId
                                         select p.PermissionName).ToListAsync();

            var finalList = permissionAllow.Concat(RolePermissions).Distinct().ToList();


            var setToRemove = new HashSet<string>(permissionDeny);
            finalList.RemoveAll(x => setToRemove.Contains(x));

            //return new APIResult(finalList);
            return finalList;
        }
    }
}
