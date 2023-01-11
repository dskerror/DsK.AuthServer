using BlazorWASMCustomAuth.PagingSortingFiltering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMCustomAuth.Shared.Security
{
    public class UsersGetDTO : PagingSortingFilteringResponse
    {
        public List<UserDTO> UserList { get; set; } = new List<UserDTO>();
    }
}
