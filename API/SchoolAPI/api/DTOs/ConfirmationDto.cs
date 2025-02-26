using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class ConfirmationDto
    {
        public string email { get; set; }       
        public int code { get; set; }
    }
}