using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Security;
using static BlazorWASMCustomAuth.Security.Shared.Constants.Access;

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

            if (model.Enabled)
            {
                var recordFind = await db.UserPermissions.Where(x => x.UserId == model.UserId && x.PermissionId == model.PermissionId).FirstOrDefaultAsync();
                if (recordFind is null)
                {
                    db.UserPermissions.Add(record);
                }
                else
                {
                    recordFind.Allow = model.Allow;
                }

                try
                {
                    recordsModifiedCount = await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.InnerException.Message;
                }

                if (recordsModifiedCount == 1)
                {
                    result.Result = recordsModifiedCount.ToString();
                    result.Message = "Record Created";
                }
            }
            else
            {
                var recordFind = await db.UserPermissions.Where(x => x.UserId == model.UserId && x.PermissionId == model.PermissionId).FirstOrDefaultAsync();
                var recordToDelete = db.UserPermissions.Attach(recordFind);
                recordToDelete.State = EntityState.Deleted;
                try
                {
                    recordsModifiedCount = await db.SaveChangesAsync();
                    result.Result = recordsModifiedCount.ToString();
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
            }

            return result;
        }
        public async Task<List<string>> GetUserPermissionsCombined(int userId)
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
        public async Task<APIResult<List<UserPermissionGridDto>>> GetUserPermissions(int userId)
        {
            APIResult<List<UserPermissionGridDto>> result = new APIResult<List<UserPermissionGridDto>>();
            var permissionList = await db.Permissions.ToListAsync();

            var permissioinGrid = Mapper.Map<List<Permission>, List<UserPermissionGridDto>>(permissionList);

            var UserPermissions = await (from u in db.Users
                                         join up in db.UserPermissions on u.Id equals up.UserId
                                         join p in db.Permissions on up.PermissionId equals p.Id
                                         where u.Id == userId
                                         select new UserPermissionGridDto()
                                         {
                                             Id = p.Id,
                                             PermissionName = p.PermissionName,
                                             PermissionDescription = p.PermissionDescription,
                                             Allow = up.Allow
                                         }).ToListAsync();

            foreach (var userPermission in UserPermissions)
            {
                var value = permissioinGrid.First(x => x.PermissionName == userPermission.PermissionName);
                value.Enabled = true;
                value.Allow = userPermission.Allow;
            }

            var RolePermissions = await (from u in db.Users
                                         join ur in db.UserRoles on u.Id equals ur.UserId
                                         join r in db.Roles on ur.RoleId equals r.Id
                                         join rp in db.RolePermissions on r.Id equals rp.RoleId
                                         join p in db.Permissions on rp.PermissionId equals p.Id
                                         where u.Id == userId
                                         select new { p.PermissionName, r.RoleName }).ToListAsync();

            foreach (var rolePermission in RolePermissions)
            {
                var value = permissioinGrid.First(x => x.PermissionName == rolePermission.PermissionName);
                if (string.IsNullOrEmpty(value.Roles))
                {
                    value.Roles = rolePermission.RoleName;
                }
                else
                {
                    value.Roles = string.Join(",", value.Roles, rolePermission.RoleName);
                }
            }

            result.Result = permissioinGrid;
            return result;
        }
    }
}
