using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public async Task<APIResult<string>> UserRoleCreate(UserRoleCreateDto model)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsCreated = 0;

            var record = new UserRole();
            Mapper.Map(model, record);

            db.UserRoles.Add(record);

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

        public async Task<APIResult<List<UserRoleDto>>> UserRolesGet(int userId)
        {
            APIResult<List<UserRoleDto>> result = new APIResult<List<UserRoleDto>>();
            var items = await db.UserRoles.Where(x => x.UserId == userId).Include(x => x.Role).ToListAsync();
            result.Result = Mapper.Map<List<UserRole>, List<UserRoleDto>>(items);
            return result;
        }

        public async Task<APIResult<string>> UserRoleDelete(int id)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsDeleted = 0;
            var record = db.UserRoles.Attach(new UserRole { Id=id });
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
