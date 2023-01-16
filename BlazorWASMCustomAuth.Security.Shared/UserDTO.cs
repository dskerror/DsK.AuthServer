using BlazorWASMCustomAuth.PagingSortingFiltering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMCustomAuth.Security.Shared
{
    public class UserDTO
    {
        public int Id { get; set; } = 0;
        public string? Username { get; set; }        
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
