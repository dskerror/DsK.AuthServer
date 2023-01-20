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
        public List<RoleModel> RolesGet()
        {
            var list = new List<RoleModel>();
            var rolesGetDt = dm.ExecDataTableSP("sp_RoleList");
            foreach (DataRow role in rolesGetDt.Rows)
            {
                list.Add(new RoleModel()
                {
                    Id = int.Parse(role["Id"].ToString() ?? ""),
                    RoleName = role["RoleName"].ToString(),
                    RoleDescription = role["RoleDescription"].ToString()
                });
            }
            return list;
        }
        public APIResult RoleCreate(RoleCreateModel model)
        {
            APIResult result = new APIResult(model);

            result.ModelValidationResult = model.ValidateModel();

            if (!result.ModelValidationResult.IsValid)
            {
                result.HasError = true;
                result.Message = "See Model Validations";
                return result;
            }

            var UserExists = RoleVerifyExistsByRoleName(model.RoleName);
            if (UserExists)
            {
                result.Message = "Role already exists.";
                result.HasError = true;
                return result;
            }

            var dbresult = dm.ExecScalarSP("sp_RoleCreate", "RoleName", model.RoleName, "RoleDescription", model.RoleDescription);
            result.Result = dbresult.Result;
            result.HasError = dbresult.HasError;
            result.Exception = dbresult.Exception;
            result.Message = "Role Created";

            return result;
        }
        public APIResult RoleUpdate(RoleModel model)
        {
            APIResult result = new APIResult(model);

            result.ModelValidationResult = model.ValidateModel();

            if (!result.ModelValidationResult.IsValid)
            {
                result.HasError = true;
                result.Message = "See Model Validations";
                return result;
            }

            var UserExists = RoleVerifyExistsByRoleName(model.RoleName);
            if (UserExists)
            {
                result.Message = "Role already exists.";
                result.HasError = true;
                return result;
            }

            var dbresult = dm.ExecScalarSP("sp_RoleUpdate", "Id", model.Id, "RoleName", model.RoleName, "RoleDescription", model.RoleDescription);
            result.Result = dbresult.Result;
            result.HasError = dbresult.HasError;
            result.Exception = dbresult.Exception;
            result.Message = "Role Updated";
            //TODO: Find if no record was update to give error message in this and other update methods

            return result;
        }
        public APIResult RoleDelete(int id)
        {
            APIResult result = new APIResult(id);

            var dbresult = dm.ExecScalarSP("sp_RoleDelete", "Id", id);
            result.Result = dbresult.Result;
            result.HasError = dbresult.HasError;
            result.Exception = dbresult.Exception;
            result.Message = "Role Deleted";
            //TODO: Find if no record was update to give error message in this and other update methods

            return result;
        }
        public bool RoleVerifyExistsByRoleName(string roleName)
        {
            var dt = dm.ExecDataTableSP("sp_RoleList", "RoleName", roleName);

            if (dt.Rows.Count == 0)
                return false;

            return true;
        }
    }
}
