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
        private DatabaseManager dm;

        public SecurityService(IOptions<TokenSettingsModel> tokenSettings, SecurityTablesTestContext db)
        {
            _tokenSettings = tokenSettings.Value;
            this.db = db;
            dm = new DatabaseManager("Server=.;Database=SecurityTablesTest;Trusted_Connection=True");
        }
    }
}
