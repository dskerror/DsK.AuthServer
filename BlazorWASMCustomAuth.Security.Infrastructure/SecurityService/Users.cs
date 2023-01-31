using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public APIResultNew<UserDto> UserCreate(UserCreateDto model)
        {
            APIResultNew<UserDto> result = new APIResultNew<UserDto>();
            int recordsCreated = 0;

            var record = new User();
            Mapper.Map(model, record);

            db.Users.Add(record);
            try
            {
                recordsCreated = db.SaveChanges();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.InnerException.Message;
            }

            result.Result = Mapper.Map(record, result.Result);
            if (recordsCreated == 1)            
                result.Message = "Record Created";
            
            return result;
        }
        public APIResultNew<List<UserDto>> UsersGet(int id = 0)
        {
            var result = new APIResultNew<List<UserDto>>();

            if (id == 0)
            {
                var userList = db.Users.ToList();
                result.Result = Mapper.Map<List<User>, List<UserDto>>(userList);

            }

            else
            {
                var userList = db.Users.Where(x => x.Id == id).ToList();
                result.Result = Mapper.Map<List<User>, List<UserDto>>(userList);
            }

            return result;
        }
        public APIResult UserUpdate(UserDto model)
        {
            APIResult result = new APIResult(model);
            int recordsUpdated = 0;
            var record = db.Users.FirstOrDefault(x => x.Id == model.Id);

            if (record != null)           
                Mapper.Map(model, record);
          

            try
            {
                recordsUpdated = db.SaveChanges();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.InnerException.Message;
            }

            result.Result = record;
            if (recordsUpdated == 1)
            {
                result.Message = "Record Updated";
            }

            return result;

        }
        public APIResult UserDelete(int id)
        {
            APIResult result = new APIResult(id);
            int recordsDeleted = 0;
            var record = db.Users.Attach(new User { Id = id });
            record.State = EntityState.Deleted;
            try
            {
                recordsDeleted = db.SaveChanges();
                
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            result.Result = recordsDeleted;

            return result;
        }
    }
}

