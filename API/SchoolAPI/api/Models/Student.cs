using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace Models
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }
        public string Name { get; set;}
        public int Age { get; set;}
        public string Address { get; set;}
        public int GPA  { get; set;}
        public ICollection<StudentSubjectGrade>? StudentSubjectGrades {get; set;}
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}