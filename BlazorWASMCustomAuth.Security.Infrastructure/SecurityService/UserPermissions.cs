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
        private List<string> GetUserPermissions(string? username)
        {
            List<string> permissions = new List<string>();

            var userPermissionsDt = dm.ExecDataTableSP("sp_UserPermissions", "Username", username ?? "");

            foreach (DataRow permission in userPermissionsDt.Rows)
            {
                permissions.Add(permission[0].ToString() ?? "");
            }

            return permissions;
        }        
    }
}
