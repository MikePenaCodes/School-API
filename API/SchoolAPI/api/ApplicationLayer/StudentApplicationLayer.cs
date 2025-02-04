using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;

namespace api.ApplicationLayer
{
    public class StudentApplicationLayer : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentApplicationLayer(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
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
        public async Task<StudentDTO> GetStudentByIdAsync(string id)
        {
            var targetStudent = await _studentRepository.GetStudentbyId(id);

            return targetStudent.ToStudentDTO();
        }

        public async Task<Student> CreateStudentAsync(CreateStudentRequestDTO studentDTO)
        {
            if (studentDTO == null)
            {
                throw new ArgumentNullException(nameof(studentDTO), "Subject data is required.");
            }

            List<TeacherSubject> targetSubjects = new List<TeacherSubject>();

            foreach (var ssg in studentDTO.StudentSubjectGrades)
            {
                var targetSubject = await _studentRepository.GetTeacherSubjects(ssg.TeacherSubjectID);
                targetSubjects.Add(targetSubject);
            }

            var student = studentDTO.ToStudentFromCreateDTO(targetSubjects);

            var studentcreated = await _studentRepository.CreateStudent(student);

            return studentcreated;

        }

        public async Task<List<StudentSubjectTeacherGradeDTO>> GetSubjectsStudentIdAsync(QueryObject query, string id)
        {
            var targetStudent = await _studentRepository.GetStudentbyId(id);

            return targetStudent.ToStudentsSubjectOnlyDTO();
        }

        public async Task<Student> EnrollSubject(QueryObject query, string id)
        {
            if (query.TeacherSubjectId == null)
            {
                throw new ArgumentNullException(nameof(query), "TeacherSubjectId is Required.");
            }

            var targetStudent = await _studentRepository.GetStudentbyId(id);
            //Gets teachersubject from database 
            var subjectFromDatabase = await _studentRepository.GetTeacherSubjects(query.TeacherSubjectId);
            //Checks if it exists.
            if (subjectFromDatabase == null)
            {
                throw new KeyNotFoundException($"Subject with ID {query.TeacherSubjectId} not found");
            }

            //checks if student is already enroll
            foreach (var ssg in targetStudent.StudentSubjectGrades)
            {
                if (ssg.TeacherSubjectID == query.TeacherSubjectId)
                {
                    throw new Exception("Students is already enrolled in this class.");
                }
                if (ssg.TeacherSubject.SubjectID == subjectFromDatabase.SubjectID)
        {
            throw new Exception("Student is already enrolled in a different class with the same subject.");
        }     
            }
            var subjectToEnroll = targetStudent.ToStudentSubject(query.TeacherSubjectId, subjectFromDatabase);

            _studentRepository.CreateStudentSubjectGrade(subjectToEnroll);

            return targetStudent;
        }

        public async Task<StudentSubjectGrade> DropClass(QueryObject query, string id)
        {
            var student = await _studentRepository.GetStudentbyId(id);

            var studentSubjectGrade = student.StudentSubjectGrades.FirstOrDefault(ssg => ssg.TeacherSubjectID == query.TeacherSubjectId);
            if(studentSubjectGrade == null)
            {
                throw new Exception("");
            }

            await _studentRepository.RemoveStudentSubject(studentSubjectGrade);

            return studentSubjectGrade;

        }

        public async Task<List<StudentSubjectGradeDTO>> GetStudentGradesByIdAsync(QueryObject query, string id)
        {
            var targetStudent = await _studentRepository.GetStudentbyId(id);

            return targetStudent.ToStudentsSubjectGradesOnlyDTO();

        }

        public async Task<List<TeacherSubjectDTO>> GetTeacherSubjects(QueryObject query)
        {
            //Calculate how many records to skip. 
            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            //Fetch paginated students from the repository
            var allTeacherSubjects = await _studentRepository.GetPaginatedTeacherSubjectsAsync(skipNumber, query.PageSize);

            //Map students to DTOs. 
            var teacherSubjectDtos = allTeacherSubjects.Select(s => s.ToTeacherSubjectDTO()).ToList();

            return teacherSubjectDtos;
        }
    }


}