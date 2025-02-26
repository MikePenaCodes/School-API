using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class StudentSubjectTeacherGradeDTO
    {
        public string? SubjectName { get; set; }
        public string? TeacherName { get; set; }
        public int? GradeNumber { get; set; }
    }
}