using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static BlazorWASMCustomAuth.Security.Shared.Constants.Access;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public async Task<APIResult<string>> UserRoleChange(UserRoleChangeDto model)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsModifiedCount = 0;
            var record = new UserRole();
            Mapper.Map(model, record);

            if (model.RoleEnabled)
            {
                db.UserRoles.Add(record);
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
                var recordFind = await db.UserRoles.Where(x=> x.UserId == model.UserId && x.RoleId == model.RoleId).FirstOrDefaultAsync();
                var recordToDelete = db.UserRoles.Attach(recordFind);
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

        public async Task<APIResult<List<UserRoleGridDto>>> UserRolesGet(int userId)
        {
            //APIResult<List<UserRoleDto>> result = new APIResult<List<UserRoleDto>>();
            //var items = await db.UserRoles.Where(x => x.UserId == userId).Include(x => x.Role).ToListAsync();
            //result.Result = Mapper.Map<List<UserRole>, List<UserRoleDto>>(items);
            //return result;



            APIResult<List<UserRoleGridDto>> result = new APIResult<List<UserRoleGridDto>>();
            var roleList = await db.ApplicationRoles.ToListAsync();


            var userPermissionList = await (from u in db.Users
                                            join ur in db.UserRoles on u.Id equals ur.UserId
                                            join r in db.ApplicationRoles on ur.RoleId equals r.Id
                                            where u.Id == userId
                                            select r.RoleName).ToListAsync();

            var roleGrid = Mapper.Map<List<ApplicationRole>, List<UserRoleGridDto>>(roleList);

            foreach (var role in userPermissionList)
            {
                var value = roleGrid.First(x => x.RoleName == role);
                value.Enable = true;
            }

            result.Result = roleGrid;
            return result;
        }

        //public async Task<APIResult<string>> UserRoleCreate(UserRoleChangeDto model)
        //{
        //    APIResult<string> result = new APIResult<string>();
        //    int recordsCreated = 0;

        //    var record = new UserRole();
        //    Mapper.Map(model, record);

        //    db.UserRoles.Add(record);

        //    try
        //    {
        //        recordsCreated = await db.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        result.HasError = true;
        //        result.Message = ex.InnerException.Message;
        //    }

        //    if (recordsCreated == 1)
        //    {
        //        result.Result = recordsCreated.ToString();
        //        result.Message = "Record Created";
        //    }

        //    return result;
        //}
        //public async Task<APIResult<string>> UserRoleDelete(int id)
        //{
        //    APIResult<string> result = new APIResult<string>();
        //    int recordsDeleted = 0;
        //    var record = db.UserRoles.Attach(new UserRole { Id=id });
        //    record.State = EntityState.Deleted;
        //    try
        //    {
        //        recordsDeleted = await db.SaveChangesAsync();
        //        result.Result = recordsDeleted.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        result.HasError = true;
        //        result.Message = ex.Message;
        //    }

        //    return result;
        //}
    }
}
