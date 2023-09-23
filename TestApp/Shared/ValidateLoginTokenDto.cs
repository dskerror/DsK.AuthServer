using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Shared;
public class ValidateLoginTokenDto
{
    public string LoginToken { get; set; } = string.Empty;
    public string TokenKey { get; set; } = string.Empty;
}
