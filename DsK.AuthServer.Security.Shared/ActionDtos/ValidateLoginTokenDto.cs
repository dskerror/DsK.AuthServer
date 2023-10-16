using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsK.AuthServer.Security.Shared.ActionDtos
{
    public class ValidateLoginTokenDto
    {
        public string LoginToken { get; set; } = string.Empty;
        public string TokenKey { get; set; } = string.Empty;
    }
}
