using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class StudentNameGradeDTO
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public int? GradeNumber { get; set;}
    }
}