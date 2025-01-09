using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class CreateStudentRequestDTO
    {
    public string Name { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
    public int GPA { get; set; }
    public List<StudentSubjectGradeDTO> StudentSubjectGrades { get; set; }
    }
}