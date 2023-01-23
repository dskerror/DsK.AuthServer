using BlazorWASMCustomAuth.Database;
using BlazorWASMCustomAuth.PagingSortingFiltering;
using BlazorWASMCustomAuth.Security.EntityFramework.Models;
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

namespace BlazorWASMCustomAuth.SecurityEF.Infrastructure
{
    public partial class SecurityServiceEF
    {   
        private readonly TokenSettingsModel _tokenSettings;
        private readonly SecurityTablesTestContext db;

        public SecurityServiceEF(IOptions<TokenSettingsModel> tokenSettings, SecurityTablesTestContext db)
        {
            _tokenSettings = tokenSettings.Value;
            this.db = db;
        }
    }
}
