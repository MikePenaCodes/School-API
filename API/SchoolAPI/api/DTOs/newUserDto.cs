using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class newUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; } 
        public string Token { get; set; }
    }
}