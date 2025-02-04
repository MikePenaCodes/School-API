using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class StudentSubjectDTO
    {
        public int TeacherSubjectID { get; set; }
        public string? SubjectName { get; set; }
    }
}