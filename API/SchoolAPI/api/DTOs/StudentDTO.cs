using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class StudentDTO
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string? GPA { get; set; } = "Not Yet Determined";
        public List<StudentSubjectGradeDTO> StudentSubjectGrades { get; set; }
    }
}