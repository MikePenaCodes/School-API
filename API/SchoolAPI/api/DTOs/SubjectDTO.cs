using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Models;

namespace api.DTOs
{
    public class SubjectDTO
    {
        public int SubjectID { get; set; }
        public string Name { get; set;}
        public int TeacherID { get; set; }
        public ICollection<StudentSubjectGrade> StudentSubjectGrades { get; set; }
        public ICollection<TeacherSubject> TeacherSubjects { get; set; }

    }
}