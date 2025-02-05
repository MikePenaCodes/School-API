using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Controllers;
using api.DTOs;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace api.ApplicationLayer
{
    public class TeacherApplicationLayer : ITeacherService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;
        public TeacherApplicationLayer(ITeacherRepository teacherRepository, IStudentRepository studentRepository)
        {
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
        }
        
        public async Task<List<TeacherDTO>> GetAllTeachersAsync(QueryObject2 query)
        {
            //Calculate how many records to skip. 
            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            var teachers = await _teacherRepository.GetPaginatedTeachersAsync(skipNumber, query.PageSize);

            var teacherDTOs = teachers.Select(t => t.ToTeacherDTO()).ToList();
            return teacherDTOs;
        }

        public async Task<TeacherDTO> GetTeacherByIdAsync(string id)
        {
            var teacher = await _teacherRepository.GetTeacherById(id);
            return teacher.ToTeacherDTO();
        }

        public async Task<Teacher> CreateTeacherAsync(CreateTeacherRequestDTO teacherDTO)
        {
            if (teacherDTO == null)
            {
                throw new ArgumentNullException(nameof(teacherDTO), "Teacher data is required.");
            }

            List<Subject> listofSubjects = new List<Subject>();

            foreach (var ts in teacherDTO.TeacherSubjects)
            {
                try
                {
                    var focusSubject = await _teacherRepository.GetSubject(ts.SubjectID);
                    listofSubjects.Add(focusSubject);
                }
                catch (Exception ex)
                {
                    throw new(ex.Message);
                }
            }

            var teacher = teacherDTO.ToTeacherFromCreateDTO();

            var teachercreated = await _teacherRepository.CreateTeacher(teacher);

            return teachercreated;
        }

        public async Task<List<SubjectDTO>> GetSubjectsAsync()
        {
            var subjects = await _teacherRepository.GetSubjects();
            var subjectDTOs = subjects.Select(s => s.ToSubjectDTO()).ToList();
            return (subjectDTOs);
        }

        public async Task<List<TeacherSubjectDTO>> GetSubjectsByTeacherIdAsync(string id)
        {

            var teacher = await _teacherRepository.GetTeacherById(id);

            if (teacher.TeacherSubjects == null)
            {
                throw new ArgumentNullException("You are not assigned to any classes.");
            }

            var teacherSubjects = teacher.ToTeacherSubjectsOnlyDTO();

            return teacherSubjects;
        }

        public async Task<SubjectDTO> CreateSubjectsByTeacherId(CreateSubjectDTO newSubjectDTO, string id)
        {
            if (newSubjectDTO == null)
            {
                throw new ArgumentNullException(nameof(newSubjectDTO), "Subject data is required.");
            }

            var teacher = await _teacherRepository.GetTeacherById(id);

            var newSubject = newSubjectDTO.ToSubjectFromCreateDTO();

            var subjectcreated = await _teacherRepository.CreateSubject(newSubject);

            return subjectcreated.ToSubjectDTO();
        }

        public async Task<List<TeacherSubjectDTO>> AssignSubjectToTeacher(QueryObject2 query, string id)
        {
            var teacher = await _teacherRepository.GetTeacherById(id);

            var subject = await _teacherRepository.GetSubject(query.SubjectId);

            foreach (var ts in teacher.TeacherSubjects)
            {
                if (ts.TeacherSubjectID == query.TeacherId & ts.SubjectID == query.SubjectId)
                {
                    throw new Exception("You are already asigned to this class.");
                }
            }
            var teacherSubject = new TeacherSubject
            {
                TeacherID = teacher.TeacherID,
                SubjectID = subject.SubjectID,
            };

            var teacherSubjectCreated = await _teacherRepository.CreateTeacherSubject(teacherSubject);

            var subjectCreatedDTO = teacher.ToTeacherSubjectsOnlyDTO();

            return subjectCreatedDTO;
        }

        public async Task<TeacherSubject> RemoveTeacherSubject(QueryObject2 query, string id)
        {
            var teacher = await _teacherRepository.GetTeacherById(id);
            var teacherSubjectFound = await _teacherRepository.GetTeacherSubjects(query.TeacherSubjectId);
            if (!(teacherSubjectFound.TeacherID == teacher.TeacherID))
            {
                throw new Exception($"You are not assigned to TeacherSubject {query.TeacherSubjectId}");
            }
            var teacherSubjectDeleted = await _teacherRepository.DeleteTeacherSubject(teacherSubjectFound);

            return teacherSubjectDeleted;
        }

        public async Task<Subject> DeleteSubjectAsync(int? id)
        {
            var subject = await _teacherRepository.GetSubject(id);

            var subjectDeleted = await _teacherRepository.DeleteSubject(subject);

            return subjectDeleted;
        }

        public async Task<TeacherSubject> GetAllStudentsBySubject(QueryObject2 query, string id)
        {
            var teacher = await _teacherRepository.GetAllTables(id);

            var teacherSubject = teacher.TeacherSubjects
            .FirstOrDefault(ts => ts.TeacherSubjectID == query.TeacherSubjectId);

            if (teacherSubject == null)
            {
                throw new KeyNotFoundException($"Subject with ID {query.TeacherSubjectId} not found for teacher with ID {query.TeacherId}");
            }

            return teacherSubject;
        }

        public async Task<StudentSubjectIDGradeDTO> UpdateGrade(UpgradeStudentSubjectGradeDTO updateDTO, string id)
        {
            if (updateDTO.GradeNumber < 0 & updateDTO.GradeNumber > 100)
            {
                throw new Exception("Invalid Grade. The Grade range is 0-100");
            }

            var teacher = await _teacherRepository.GetAllTables(id);

            var updatedStudentGrade = teacher.UpdateStudentsGrade(updateDTO.TeacherSubjectID, updateDTO.StudentID, updateDTO.GradeNumber);

            if (updatedStudentGrade == null)
            {
                throw new KeyNotFoundException($"Student with ID {updateDTO.StudentID} is not enrolled in Subject with ID {updateDTO.TeacherSubjectID} ");
            }

            await _teacherRepository.UpdateGrade();

            return updatedStudentGrade;

        }

        public async Task<List<StudentDTO>> GetAllStudentsAsync(QueryObject query)
        {
            //Calculate how many records to skip. 
            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            //Fetch paginated students from the repository
            var allStudents = await _studentRepository.GetPaginatedStudentsAsync(skipNumber, query.PageSize);

            //Map students to DTOs. 
            var studentDtos = allStudents.Select(s => s.ToStudentDTO()).ToList();

            return studentDtos;
        }
    }
}
