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
        public APIResult UserCreateLocalPassword(UserCreateLocalPasswordModel u)
        {
            //TODO : Implement Password Complexity Rules
            //TODO : Implement Previously Used Password Constraint

            APIResult result = new APIResult(u);

            var ramdomSalt = SecurityHelpers.RandomizeSalt;

            var dbresult = dm.ExecScalarSP("sp_UserPasswordCreate",
                "UserId", u.UserId,
                "HashedPassword", SecurityHelpers.HashPasword(u.Password, ramdomSalt),
                "Salt", Convert.ToHexString(ramdomSalt));
            result.Result = dbresult.Result;
            result.HasError = dbresult.HasError;
            result.Exception = dbresult.Exception;
            result.Message = "Password Created";

            return result;
        }
    }
}
