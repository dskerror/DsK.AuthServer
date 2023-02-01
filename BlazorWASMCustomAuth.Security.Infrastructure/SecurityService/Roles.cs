using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public APIResult<string> RoleCreate(RoleCreateDto model)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsCreated = 0;

            var record = new Role();
            Mapper.Map(model, record);

            db.Roles.Add(record);

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

        public APIResult<List<RoleDto>> RolesGet(int id = 0)
        {
            APIResult<List<RoleDto>> result = new APIResult<List<RoleDto>>();
            if (id == 0)
            {
                var items = db.Roles.ToList();
                result.Result = Mapper.Map<List<Role>, List<RoleDto>>(items);

            }
            else
            {
                var items = db.Roles.Where(x => x.Id == id).ToList();
                result.Result = Mapper.Map<List<Role>, List<RoleDto>>(items);
            }

            return result;
        }

        public APIResult<string> RoleUpdate(RoleUpdateDto model)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsUpdated = 0;
            var record = db.Roles.FirstOrDefault(x => x.Id == model.Id);

            if (record != null)
            {
                Mapper.Map(model, record);
            }

            try
            {
                recordsUpdated = db.SaveChanges();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.InnerException.Message;
            }
                        
            if (recordsUpdated == 1)
            {
                result.Result = recordsUpdated.ToString();
                result.Message = "Record Updated";
            }

            return result;
        }
        public APIResult<string> RoleDelete(int id)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsDeleted = 0;
            var record = db.Roles.Attach(new Role { Id = id });
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
