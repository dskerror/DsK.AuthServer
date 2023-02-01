using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public APIResult<string> AuthenticationProvidersCreate(AuthenticationProviderCreateDto model)
        {
            APIResult<string> result = new APIResult<string>();
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

            result.Result = recordsCreated.ToString();
            if (recordsCreated == 1)
            {
                result.Message = "Record Created";
            }

            return result;
        }

        public APIResult<List<AuthenticationProviderDto>> AuthenticationProvidersGet(int id = 0)
        {
            APIResult<List<AuthenticationProviderDto>> result = new APIResult<List<AuthenticationProviderDto>>();
            if (id == 0)
            {
                var items = db.AuthenticationProviders.ToList();
                result.Result = Mapper.Map<List<AuthenticationProvider>, List<AuthenticationProviderDto>>(items);
            }
            else
            {
                var items = db.AuthenticationProviders.Where(x => x.Id == id).ToList();
                result.Result = Mapper.Map<List<AuthenticationProvider>, List<AuthenticationProviderDto>>(items);
            }

            return result;
        }

        public APIResult<string> AuthenticationProvidersUpdate(AuthenticationProviderUpdateDto model)
        {
            APIResult<string> result = new APIResult<string>();
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

            if (recordsUpdated == 1)
            {
                result.Result = recordsUpdated.ToString();
                result.Message = "Record Updated";
            }

            return result;
        }
        public APIResult<string> AuthenticationProvidersDelete(int id)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsDeleted = 0;
            var record = db.AuthenticationProviders.Attach(new AuthenticationProvider { Id = id });
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
