using AutoMapper;
using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using DsK.Services.Email;
using Microsoft.Extensions.Options;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {   
        private readonly TokenSettingsModel _tokenSettings;
        private readonly SecurityTablesTestContext db;
        private IMapper Mapper;
        private readonly IMailService _mailService;

        public SecurityService(
            IOptions<TokenSettingsModel> tokenSettings, 
            SecurityTablesTestContext db, 
            IMapper Mapper, 
            IMailService mailService)
        {
            _tokenSettings = tokenSettings.Value;
            this.db = db;
            this.Mapper= Mapper;
            _mailService = mailService;
        }
    }
}
