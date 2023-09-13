using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMCustomAuth.Security.Shared.Requests;

public class ApplicationPagedRequest : PagedRequest
{
    public int ApplicationId { get; set; } = 0;
}
