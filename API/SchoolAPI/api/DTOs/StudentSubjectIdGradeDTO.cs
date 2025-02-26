using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class StudentSubjectIDGradeDTO
    {
    public int StudentID { get; set; }
    public string? StudentName { get; set;}
    public int? GradeNumber { get; set; }
    }
}