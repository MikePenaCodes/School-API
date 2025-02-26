using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace Models
{
    public class StudentSubjectGrade
{
    // Foreign keys
    public int StudentID { get; set; }
    public int TeacherSubjectID { get; set; }

    // Grade for the student in this subject
    public int? GradeNumber { get; set; }

    // Navigation properties
    public required Student Student { get; set; }
    public required TeacherSubject TeacherSubject { get; set; } 
    
    
}

}