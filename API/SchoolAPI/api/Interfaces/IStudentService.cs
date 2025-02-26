using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Helpers;
using Models;

namespace api.Interfaces
{
    public interface IStudentService
    {
    
    Task<StudentDTO> GetStudentByIdAsync(string id);   //Get Student by Id
    Task<Student> CreateStudentAsync(CreateStudentRequestDTO studentDTO); //Create a student
    Task<List<StudentSubjectTeacherGradeDTO>> GetSubjectsStudentIdAsync(QueryObject query, string id); //Get Subjects by StudentId 
    Task<Student> EnrollSubject(QueryObject query, string id);//Enroll student in a subject
    Task<StudentSubjectGrade> DropClass(QueryObject query, string id);  //Drop class 
    Task<List<StudentSubjectGradeDTO>> GetStudentGradesByIdAsync(QueryObject query, string id);  //Get Grades from Student
    Task<List<TeacherSubjectDTO>> GetTeacherSubjects(QueryObject query);
    
        
    }
}