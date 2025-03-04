using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class CreateTeacherRequestDTO
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public int YearsofExp   { get; set; }
        public List<TeacherSubjectDTO> TeacherSubjects { get; set; }
    }
}