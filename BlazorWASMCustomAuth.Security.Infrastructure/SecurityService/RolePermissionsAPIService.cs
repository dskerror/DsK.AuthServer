using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public async Task<APIResult<string>> RolePermissionChange(RolePermissionChangeDto model)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsModifiedCount = 0;
            var record = new ApplicationRolePermission();
            Mapper.Map(model, record);

            if (model.PermissionEnabled)
            {
                db.ApplicationRolePermissions.Add(record);
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
                var recordToDelete = db.ApplicationRolePermissions.Attach(record);
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


        public async Task<APIResult<List<ApplicationRolePermissionGridDto>>> RolePermissionsGet(int roleId)
        {

            APIResult<List<ApplicationRolePermissionGridDto>> result = new APIResult<List<ApplicationRolePermissionGridDto>>();
            var permissionList = await db.ApplicationPermissions.ToListAsync();


            var RolePermissionList = await (from r in db.ApplicationRoles
                                            join rp in db.ApplicationRolePermissions on r.Id equals rp.RoleId
                                            join p in db.ApplicationPermissions on rp.PermissionId equals p.Id
                                            where r.Id == roleId
                                            select p.PermissionName).ToListAsync();

            var permissionGrid = Mapper.Map<List<ApplicationPermission>, List<ApplicationRolePermissionGridDto>>(permissionList);

            foreach (var permission in RolePermissionList)
            {
                var value = permissionGrid.First(x => x.PermissionName == permission);
                value.Allow = true;
            }

            result.Result = permissionGrid;
            return result;

        }
    }
}
