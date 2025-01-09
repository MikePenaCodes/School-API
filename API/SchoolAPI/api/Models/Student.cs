using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string Name { get; set;}
        public int Age { get; set;}
        public string Address { get; set;}
        public int GPA  { get; set;}
        public ICollection<StudentSubjectGrade>? StudentSubjectGrades {get; set;}
    }
}