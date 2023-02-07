using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public async Task<APIResult<string>> RolePermissionCreate(RolePermissionCreateDto model)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsCreated = 0;

            var record = new RolePermission();
            Mapper.Map(model, record);

            db.RolePermissions.Add(record);

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

        public async Task<APIResult<List<RolePermissionGridDto>>> RolePermissionsGet(int roleId)
        {

            APIResult<List<RolePermissionGridDto>> result = new APIResult<List<RolePermissionGridDto>>();
            var permissionList = await db.Permissions.ToListAsync();


            var RolePermissionList = await (from r in db.Roles
                                            join rp in db.RolePermissions on r.Id equals rp.RoleId
                                            join p in db.Permissions on rp.PermissionId equals p.Id
                                            where r.Id == roleId
                                            select p.PermissionName).ToListAsync();

            var permissionGrid = Mapper.Map<List<Permission>, List<RolePermissionGridDto>>(permissionList);

            foreach (var permission in RolePermissionList)
            {
                var value = permissionGrid.First(x => x.PermissionName == permission);
                value.Enable = true;
            }

            result.Result = permissionGrid;
            return result;

        }
        //public async Task<APIResult<List<RolePermissionDto>>> RolePermissionsGet(int id = 0)
        //{
        //    APIResult<List<RolePermissionDto>> result = new APIResult<List<RolePermissionDto>>();
        //    if (id == 0)
        //    {
        //        var items = await db.RolePermissions.ToListAsync();
        //        result.Result = Mapper.Map<List<RolePermission>, List<RolePermissionDto>>(items);
        //    }
        //    else
        //    {
        //        var items = db.RolePermissions.Where(x => x.RoleId == id).ToList();
        //        result.Result = Mapper.Map<List<RolePermission>, List<RolePermissionDto>>(items);
        //    }

        //    return result;
        //}

        public async Task<APIResult<string>> RolePermissionDelete(RolePermissionDeleteDto model)
        {
            APIResult<string> result = new APIResult<string>();

            var recordToDelete = new RolePermission();
            Mapper.Map(model, recordToDelete);

            int recordsDeleted = 0;
            var record = db.RolePermissions.Attach(recordToDelete);
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
