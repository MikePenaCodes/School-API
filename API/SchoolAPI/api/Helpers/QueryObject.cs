using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class QueryObject
    {
        public int? StudentId {get; set;} = null;
        public int? TeacherSubjectId {get; set;} = null;
        public int? SubjectId {get; set;} = null;
        public int PageNumber {get; set;} = 1;
        public int PageSize {get;set;} = 10;
        public string? SubjectName {get; set;} = null;
        public string? TeacherName {get; set;} = null;

        
    }
}