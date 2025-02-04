using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class NewPassWordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }       
        public string Code { get; set; }
        [Required]
        public string Password { get; set; }
    }
}