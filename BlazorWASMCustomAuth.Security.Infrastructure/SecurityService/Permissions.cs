using BlazorWASMCustomAuth.Database;
using BlazorWASMCustomAuth.PagingSortingFiltering;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Validations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public List<PermissionModel> PermissionsGet()
        {
            var list = new List<PermissionModel>();
            var permissionsGetDt = dm.ExecDataTableSP("sp_PermissionList");
            foreach (DataRow permission in permissionsGetDt.Rows)
            {
                list.Add(new PermissionModel()
                {
                    Id = int.Parse(permission["Id"].ToString() ?? ""),
                    PermissionName = permission["PermissionName"].ToString(),
                    PermissionDescription = permission["PermissionDescription"].ToString()
                });
            }

            return list;
        }
        public APIResult PermissionCreate(PermissionCreateModel model)
        {
            APIResult result = new APIResult(model);

            result.ModelValidationResult = model.ValidateModel();

            if (!result.ModelValidationResult.IsValid)
            {
                result.HasError = true;
                result.Message = "See Model Validations";
                return result;
            }

            var UserExists = PermissionVerifyExistsByPermissionName(model.PermissionName);
            if (UserExists)
            {
                result.Message = "Permission already exists.";
                result.HasError = true;
                return result;
            }

            var dbresult = dm.ExecNonQuerySP("sp_PermissionCreate", "PermissionName", model.PermissionName, "PermissionDescription", model.PermissionDescription);
            result.Result = dbresult.Result;
            result.HasError = dbresult.HasError;
            result.Exception = dbresult.Exception;
            result.Message = "Permission Created";

            return result;
        }
        public APIResult PermissionUpdate(PermissionModel model)
        {
            APIResult result = new APIResult(model);

            result.ModelValidationResult = model.ValidateModel();

            if (!result.ModelValidationResult.IsValid)
            {
                result.HasError = true;
                result.Message = "See Model Validations";
                return result;
            }

            var UserExists = PermissionVerifyExistsByPermissionName(model.PermissionName);
            if (UserExists)
            {
                result.Message = "Permission already exists.";
                result.HasError = true;
                return result;
            }

            var dbresult = dm.ExecNonQuerySP("sp_PermissionUpdate", "Id", model.Id, "PermissionName", model.PermissionName, "PermissionDescription", model.PermissionDescription);
            result.Result = dbresult.Result;
            result.HasError = dbresult.HasError;
            result.Exception = dbresult.Exception;
            result.Message = "Permission Updated";

            return result;
        }
        public APIResult PermissionDelete(int id)
        {
            APIResult result = new APIResult(id);

            var dbresult = dm.ExecNonQuerySP("sp_PermissionDelete", "Id", id);
            result.Result = dbresult.Result;
            result.HasError = dbresult.HasError;
            result.Exception = dbresult.Exception;
            result.Message = "Permission Deleted";
            //TODO: Find if no record was update to give error message in this and other update methods

            return result;
        }
        public bool PermissionVerifyExistsByPermissionName(string permissionName)
        {
            var dt = dm.ExecDataTableSP("sp_PermissionList", "PermissionName", permissionName);

            if (dt.Rows.Count == 0)
                return false;

            return true;
        }

    }
}
