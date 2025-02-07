using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace Models
{
    public class Subject
    {
        public int SubjectID { get; set; }
        public string Name { get; set; }
        public int Hours { get; set; }
        public ICollection<TeacherSubject>? TeacherSubjects { get; set; }

    }
}