using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace api.Models
{
    public class TeacherSubject
    {
        public int TeacherSubjectID { get; set; }
        public int TeacherID { get; set; }
        public int SubjectID { get; set; }

        // Navigation properties
        public Teacher? Teacher { get; set; }
        public Subject? Subject { get; set; }
        public ICollection<StudentSubjectGrade>? StudentSubjectGrades { get; set; }

    }
}