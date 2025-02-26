using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Models;

namespace api.Models
{
    public class AppUser : IdentityUser
    {
        //Navigation Properties
        // public Student? Student { get; set; }
        // public Teacher? Teacher { get; set; }
    }
}