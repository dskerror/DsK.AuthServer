using AutoMapper;
using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public APIResult UserCreate(UserCreateDto model)
        {
            APIResult result = new APIResult(model);
            int recordsCreated = 0;

            var record = new User();
            Mapper.Map(model, record);
            //var record = new User()
            //{
            //    Username = model.Username,
            //    Email = model.Email,
            //    Name = model.Name
            //};

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

            result.Result = record;
            if (recordsCreated == 1)
            {
                result.Message = "Record Created";
            }

            return result;
        }

        public APIResult UsersGet(int id = 0)
        {
            APIResult result = new APIResult(id);
            if (id == 0)
                result.Result = db.Users.ToList();
            else
                result.Result = db.Users.Where(x => x.Id == id).FirstOrDefault();

            return result;
        }

        public APIResult UserUpdate(UserUpdateDto model)
        {
            APIResult result = new APIResult(model);
            int recordsUpdated = 0;
            var record = db.Users.FirstOrDefault(x => x.Id == model.Id);

            if (record != null)
            {
                record.Email = model.Email;
                record.Name = model.Name;
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

            return result;
        }
    }
}

