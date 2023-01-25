using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public APIResult AuthenticationProvidersCreate(AuthenticationProviderCreateDto model)
        {
            APIResult result = new APIResult(model);
            int recordsCreated = 0;

            var record = new AuthenticationProvider();
            Mapper.Map(model, record);

            db.AuthenticationProviders.Add(record);

            try
            {
                recordsCreated = db.SaveChanges();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.InnerException.Message;
            }

            result.Result = recordsCreated;
            if (recordsCreated == 1)
            {
                result.Message = "Record Created";
            }

            return result;
        }

        public APIResult AuthenticationProvidersGet(int id = 0)
        {
            APIResult result = new APIResult(id);
            if (id == 0)
                result.Result = db.AuthenticationProviders.ToList();
            else
                result.Result = db.AuthenticationProviders.Where(x => x.Id == id).FirstOrDefault();

            return result;
        }

        public APIResult AuthenticationProvidersUpdate(AuthenticationProviderUpdateDto model)
        {
            APIResult result = new APIResult(model);
            int recordsUpdated = 0;
            var record = db.AuthenticationProviders.FirstOrDefault(x => x.Id == model.Id);

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

            result.Result = record;
            if (recordsUpdated == 1)
            {
                result.Message = "Record Updated";
            }

            return result;
        }
        public APIResult AuthenticationProvidersDelete(int id)
        {
            APIResult result = new APIResult(id);
            int recordsDeleted = 0;
            var record = db.AuthenticationProviders.Attach(new AuthenticationProvider { Id = id });
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
