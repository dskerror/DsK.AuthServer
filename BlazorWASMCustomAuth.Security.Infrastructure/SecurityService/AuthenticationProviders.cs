using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Data;

namespace BlazorWASMCustomAuth.Security.Infrastructure{
    public partial class SecurityService
    {
        public APIResult<AuthenticationProviderDto> AuthenticationProvidersCreate(AuthenticationProviderCreateDto model)
        {
            APIResult<AuthenticationProviderDto> result = new APIResult<AuthenticationProviderDto>();
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
            
            if (recordsCreated == 1)
            {
                result.Result = Mapper.Map(record, result.Result);
                result.Message = "Record Created";
            }

            return result;
        }
        public APIResult<List<AuthenticationProviderDto>> AuthenticationProvidersGet(int id, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            var result = new APIResult<List<AuthenticationProviderDto>>();

            string ordering = "Id";
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                string[] OrderBy = orderBy.Split(',');
                ordering = string.Join(",", OrderBy);
            }
            result.Paging.CurrentPage = pageNumber;
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 10 : pageSize;
            int count = 0;
            List<AuthenticationProvider> items;
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                count = db.AuthenticationProviders
                    .Where(m => m.AuthenticationProviderName.Contains(searchString) || m.AuthenticationProviderType.Contains(searchString))
                    .Count();

                items = db.AuthenticationProviders.OrderBy(ordering)
                    .Where(m => m.AuthenticationProviderName.Contains(searchString) || m.AuthenticationProviderType.Contains(searchString))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            else if (id != 0)
            {
                count = db.AuthenticationProviders
                    .Where(u => u.Id == id)
                    .Count();

                items = db.AuthenticationProviders.OrderBy(ordering)
                    .Where(u => u.Id == id)
                    .ToList();
            }
            else
            {
                count = db.AuthenticationProviders.Count();

                items = db.AuthenticationProviders.OrderBy(ordering)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            result.Paging.TotalItems = count;
            result.Result = Mapper.Map<List<AuthenticationProvider>, List<AuthenticationProviderDto>>(items);
            return result;
        }
        public APIResult<string> AuthenticationProvidersUpdate(AuthenticationProviderUpdateDto model)
        {
            APIResult<string> result = new APIResult<string>();

            int recordsUpdated = 0;
            var record = db.AuthenticationProviders.FirstOrDefault(x => x.Id == model.Id);

            if(record.AuthenticationProviderName == "Local")
            {
                result.HasError = true;
                result.Message = "Local Authentication Provider can't be updated";
            }

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

            if (recordsUpdated == 1)
                result.Message = "Record Updated";            

            return result;
        }
        public APIResult<string> AuthenticationProvidersDelete(int id)
        {
            APIResult<string> result = new APIResult<string>();
            int recordsDeleted = 0;

            var record = db.AuthenticationProviders.FirstOrDefault(x => x.Id == id);

            try
            {
                if (record.AuthenticationProviderName == "Local")
                {
                    result.HasError = true;
                    result.Message = "Local Authentication Provider can't be deleted";
                } else
                {
                    db.Remove(record);
                    recordsDeleted = db.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            result.Result = recordsDeleted.ToString();

            return result;
        }
    }
}
