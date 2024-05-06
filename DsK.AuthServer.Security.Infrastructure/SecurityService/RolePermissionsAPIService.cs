using DsK.AuthServer.Security.EntityFramework.Models;
using DsK.AuthServer.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DsK.AuthServer.Security.Infrastructure
{
    public partial class SecurityService
    {
        public async Task<APIResponse<string>> ApplicationRolePermissionChange(ApplicationRolePermissionChangeDto model)
        {
            APIResponse<string> result = new APIResponse<string>();
            int recordsModifiedCount = 0;
            var record = new ApplicationRolePermission();
            Mapper.Map(model, record);

            if (model.IsEnabled)
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


        public async Task<APIResponse<List<ApplicationRolePermissionGridDto>>> ApplicationRolePermissionsGet(int ApplicationId, int ApplicationRoleId)
        {

            APIResponse<List<ApplicationRolePermissionGridDto>> result = new APIResponse<List<ApplicationRolePermissionGridDto>>();
            var permissionList = await db.ApplicationPermissions.Where(x => x.ApplicationId == ApplicationId).ToListAsync();


            var RolePermissionList = await (from r in db.ApplicationRoles
                                            join rp in db.ApplicationRolePermissions on r.Id equals rp.ApplicationRoleId
                                            join p in db.ApplicationPermissions on new { Id = rp.ApplicationPermissionId, r.ApplicationId } equals new { p.Id, p.ApplicationId } //rp.ApplicationPermissionId equals p.Id r.ApplicationId equals p.ApplicationId
                                            where r.ApplicationId == ApplicationId && r.Id == ApplicationRoleId
                                            select p.PermissionName).ToListAsync();

            var permissionGrid = Mapper.Map<List<ApplicationPermission>, List<ApplicationRolePermissionGridDto>>(permissionList);

            foreach (var permission in RolePermissionList)
            {
                var value = permissionGrid.First(x => x.PermissionName == permission);
                value.Overwrite = true;
            }

            result.Result = permissionGrid;
            return result;
        }
    }
}
