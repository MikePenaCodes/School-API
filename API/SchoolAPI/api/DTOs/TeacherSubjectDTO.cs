using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class TeacherSubjectDTO
    {
        public int TeacherSubjectID { get; set;}
        public string TeacherName { get; set;}
        public int SubjectID {get;set;}
        public string? SubjectName {get;set;} 
        
    }
}