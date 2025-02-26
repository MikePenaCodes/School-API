using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace api.Interfaces
{
    public interface ITeacherRepository
    {
        Task<List<Teacher>> GetPaginatedTeachersAsync(int skip, int pageSize);
        Task<Teacher> GetTeacherById(string id);
        Task<Teacher> CreateTeacher(Teacher teacher);
        Task<Subject> GetSubject(int? id);
        Task<List<Subject>> GetSubjects();
        Task<TeacherSubject> GetTeacherSubjects(int? id);
        Task<Subject> CreateSubject(Subject subject);
        Task<TeacherSubject> CreateTeacherSubject(TeacherSubject teachersubject);
        Task<TeacherSubject> DeleteTeacherSubject(TeacherSubject teacherSubject);
        Task<Subject> DeleteSubject(Subject subject);
        Task<Teacher> GetAllTables(string teacherId);
        Task UpdateGrade();


    }
}