using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class UpgradeStudentSubjectGradeDTO
    {
        public int StudentID { get; set; }
        [Required]
        public int GradeNumber { get; set; }
        public int TeacherSubjectID {get; set; }

    }
}