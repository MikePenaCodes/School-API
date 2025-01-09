using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class StudentSubjectGrade
{
    // Foreign keys
    public int StudentID { get; set; }
    public int SubjectID { get; set; }

    // Grade for the student in this subject
    public int? GradeNumber { get; set; }

    // Navigation properties
    public required Student Student { get; set; }
    public required Subject Subject { get; set; } 
    
    
}

}