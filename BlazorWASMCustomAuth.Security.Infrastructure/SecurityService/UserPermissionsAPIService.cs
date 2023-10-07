using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using static BlazorWASMCustomAuth.Security.Shared.Access;

namespace BlazorWASMCustomAuth.Security.Infrastructure;
public partial class SecurityService
{
    public async Task<APIResult<string>> UserPermissionChange(UserPermissionChangeDto model)
    {
        APIResult<string> result = new APIResult<string>();
        int recordsModifiedCount = 0;

        var record = new UserPermission();
        Mapper.Map(model, record);

        if (model.IsEnabled)
        {
            var recordFind = await db.UserPermissions.Where(x => x.UserId == model.UserId && x.PermissionId == model.PermissionId).FirstOrDefaultAsync();
            if (recordFind is null)
            {
                db.UserPermissions.Add(record);
            }
            else
            {
                recordFind.Overwrite = model.Overwrite;
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
                                     join p in db.ApplicationPermissions on up.PermissionId equals p.Id
                                     where u.Id == userId && up.Overwrite == true
                                     select p.PermissionName).ToListAsync();


        var permissionDeny = await (from u in db.Users
                                    join up in db.UserPermissions on u.Id equals up.UserId
                                    join p in db.ApplicationPermissions on up.PermissionId equals p.Id
                                    where u.Id == userId && up.Overwrite == false
                                    select p.PermissionName).ToListAsync();

        var RolePermissions = await (from u in db.Users
                                     join ur in db.UserRoles on u.Id equals ur.UserId
                                     join r in db.ApplicationRoles on ur.RoleId equals r.Id
                                     join rp in db.ApplicationRolePermissions on r.Id equals rp.ApplicationRoleId
                                     join p in db.ApplicationPermissions on rp.ApplicationPermissionId equals p.Id
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
        var applicationPermissions = await db.ApplicationPermissions.Include(x => x.Application).ToListAsync();

        //var permissioinGrid = Mapper.Map<List<ApplicationPermission>, List<UserPermissionGridDto>>(permissionList);

        var userPermissions = await (from u in db.Users
                                     join up in db.UserPermissions on u.Id equals up.UserId
                                     join p in db.ApplicationPermissions on up.PermissionId equals p.Id
                                     where u.Id == userId
                                     select new UserPermissionGridDto()
                                     {
                                         Id = p.Id,
                                         PermissionName = p.PermissionName,
                                         PermissionDescription = p.PermissionDescription,
                                         Overwrite = up.Overwrite
                                     }).ToListAsync();

        List<UserPermissionGridDto> userPermissionGridDto = new List<UserPermissionGridDto>();

        //foreach (var userPermission in UserPermissions)
        //{
        //    var value = permissioinGrid.First(x => x.PermissionName == userPermission.PermissionName);
        //    value.Enabled = true;
        //    value.Allow = userPermission.Allow;
        //}


        foreach (var permission in applicationPermissions)
        {
            var value = new UserPermissionGridDto()
            {
                Id = permission.Id,
                ApplicationName = permission.Application.ApplicationName,
                ApplicationId = permission.Application.Id,
                PermissionName = permission.PermissionName,
                PermissionDescription = permission.PermissionDescription
            };

            var lookupInUserPermission = userPermissions.Where(x => x.PermissionName == permission.PermissionName && x.Id == permission.ApplicationId).FirstOrDefault();
            if (lookupInUserPermission != null) { value.IsEnabled = true; }

            userPermissionGridDto.Add(value);
        }


        var RolePermissions = await (from u in db.Users
                                     join ur in db.UserRoles on u.Id equals ur.UserId
                                     join r in db.ApplicationRoles on ur.RoleId equals r.Id
                                     join rp in db.ApplicationRolePermissions on r.Id equals rp.ApplicationRoleId
                                     join p in db.ApplicationPermissions on rp.ApplicationPermissionId equals p.Id
                                     join a in db.Applications on p.ApplicationId equals a.Id
                                     where u.Id == userId
                                     select new { p.PermissionName, r.RoleName, ApplicationId = a.Id }).ToListAsync();

        foreach (var rolePermission in RolePermissions)
        {
            var value = userPermissionGridDto.First(x => x.PermissionName == rolePermission.PermissionName && x.ApplicationId == rolePermission.ApplicationId);
            if (string.IsNullOrEmpty(value.Roles))
            {
                value.Roles = rolePermission.RoleName;
            }
            else
            {
                value.Roles = string.Join(",", value.Roles, rolePermission.RoleName);
            }
        }

        result.Result = userPermissionGridDto;
        return result;
    }
}
