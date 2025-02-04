using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class StudentNamesDTO
    {
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public List<StudentNameGradeDTO?> Students { get; set;}
    }
}