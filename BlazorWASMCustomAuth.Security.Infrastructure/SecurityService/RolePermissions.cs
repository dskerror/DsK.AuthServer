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
    }
}
