using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Helpers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace api.Interfaces 
{
    public interface ITeacherService
    {
    Task<List<TeacherDTO>> GetAllTeachersAsync(QueryObject2 query);
    Task<TeacherDTO> GetTeacherByIdAsync(string id);
    Task<Teacher> CreateTeacherAsync(CreateTeacherRequestDTO teacherDTO);
    Task<List<SubjectDTO>> GetSubjectsAsync();
    Task<List<TeacherSubjectDTO>> GetSubjectsByTeacherIdAsync(string id);
    Task<SubjectDTO> CreateSubjectsByTeacherId(CreateSubjectDTO newSubjectDTO, string id);
    Task<List<TeacherSubjectDTO>> AssignSubjectToTeacher(QueryObject2 query, string id);
    Task<TeacherSubject> RemoveTeacherSubject(QueryObject2 query, string id);
    Task<Subject> DeleteSubjectAsync(int? subjectId);
    Task<TeacherSubject> GetAllStudentsBySubject(QueryObject2 query, string id);
    Task<StudentSubjectIDGradeDTO> UpdateGrade(UpgradeStudentSubjectGradeDTO updateDTO, string id);
    Task<List<StudentDTO>> GetAllStudentsAsync(QueryObject query);  //Get All Studentsd
    }
}