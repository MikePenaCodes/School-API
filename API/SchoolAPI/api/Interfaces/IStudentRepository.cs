using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace api.Interfaces
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetPaginatedStudentsAsync(int skip, int pageSize);
        Task<Student> GetStudentbyId(string id);
        Task RemoveStudentSubject(StudentSubjectGrade studentSubject);
        Task<TeacherSubject> GetTeacherSubjects(int? id);
        Task<Student> CreateStudent(Student student);
        Task CreateStudentSubjectGrade(StudentSubjectGrade studentSubject);
        Task<List<TeacherSubject>> GetPaginatedTeacherSubjectsAsync(int skip, int PageSize);

    }
}