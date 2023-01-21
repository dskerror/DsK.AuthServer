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
        public List<RolePermissionModel> RolePermissionsGet()
        {
            var list = new List<RolePermissionModel>();
            var dt = dm.ExecDataTableSP("sp_RolePermissionsGet");
            foreach (DataRow role in dt.Rows)
            {
                list.Add(new RolePermissionModel()
                {
                    RoleId = int.Parse(role["RoleId"].ToString() ?? ""),
                    RoleName = role["RoleName"].ToString(),
                    RoleDescription = role["RoleDescription"].ToString(),
                    PermissionId = int.Parse(role["PermissionId"].ToString() ?? ""),
                    PermissionName = role["PermissionName"].ToString(),
                    PermissionDescription = role["PermissionDescription"].ToString()
                });
            }

            return list;
        }

        public APIResult RolePermissionCreate(RolePermissionDto model)
        {
            APIResult result = new APIResult(model);

            var UserExists = VerifyRolePermissionExists(model.RoleId, model.PermissionId);
            if (UserExists)
            {
                result.Message = "Permission already exists in Role.";
                result.HasError = true;
                return result;
            }

            var dbresult = dm.ExecNonQuerySP("sp_RolePermissionCreate", "RoleId", model.RoleId, "PermissionId", model.PermissionId);
            result.Result = dbresult.Result;
            result.HasError = dbresult.HasError;
            result.Exception = dbresult.Exception;
            result.Message = "Role Permission Created";

            return result;
        }
        
        public APIResult RolePermissionDelete(RolePermissionDto model)
        {
            APIResult result = new APIResult(model);

            var dbresult = dm.ExecNonQuerySP("sp_RolePermissionDelete", "RoleId", model.RoleId, "PermissionId", model.PermissionId);
            result.Result = dbresult.Result;
            result.HasError = dbresult.HasError;
            result.Exception = dbresult.Exception;
            result.Message = "Permission Deleted";            

            return result;
        }

        public bool VerifyRolePermissionExists(int roleId, int permissionId)
        {
            var dt = dm.ExecDataTableSP("sp_RolePermissionList", "RoleId", roleId, "PermissionId", permissionId);

            if (dt.Rows.Count == 0)
                return false;

            return true;
        }
    }
}
