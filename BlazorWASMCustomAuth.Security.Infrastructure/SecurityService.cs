using AutoMapper;
using BlazorWASMCustomAuth.Database;
using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.Extensions.Options;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {   
        private readonly TokenSettingsModel _tokenSettings;
        private readonly SecurityTablesTestContext db;
        //private DatabaseManager dm;
        private IMapper Mapper;

        public SecurityService(IOptions<TokenSettingsModel> tokenSettings, SecurityTablesTestContext db, IMapper Mapper)
        {
            _tokenSettings = tokenSettings.Value;
            this.db = db;
            this.Mapper= Mapper;
            //dm = new DatabaseManager("Server=.;Database=SecurityTablesTest;Trusted_Connection=True");
        }
    }
}
