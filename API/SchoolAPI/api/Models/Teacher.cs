using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace Models
{
    public class Teacher
    {
        [Key]
        public int TeacherID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public int YearsofExp   { get; set; }
        public ICollection<TeacherSubject>? TeacherSubjects { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}