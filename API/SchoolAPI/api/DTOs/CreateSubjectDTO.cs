using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Models;

namespace api.DTOs
{
    public class CreateSubjectDTO
    {
        public required string Name { get; set; }
        public required int Hours { get; set; }
        // public ICollection<StudentSubjectGradeDTO>? StudentSubjectGrades { get; set; }
        // public ICollection<TeacherSubjectDTO>? TeacherSubjects { get; set; }

    }
}