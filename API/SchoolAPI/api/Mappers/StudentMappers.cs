using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;
using Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Models;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;

namespace api.Mappers
{
    public static class StudentMappers
    {
        public static StudentDTO ToStudentDTO(this Student studentModel)
        {
            return new StudentDTO
            {
                StudentID = studentModel.StudentID,
                Name = studentModel.Name,
                Age = studentModel.Age,
                Address = studentModel.Address,
                GPA = studentModel.GPA,
                StudentSubjectGrades = studentModel.StudentSubjectGrades.Select(ssg => new StudentSubjectGradeDTO
                {
                    TeacherSubjectID = ssg.TeacherSubjectID,
                    SubjectName = ssg.TeacherSubject.Subject.Name,
                    GradeNumber = ssg.GradeNumber
                })
        .ToList()
            };
        }
        public static StudentDTO ToStudentnogradeDTO(this Student studentModel)
        {
            return new StudentDTO
            {
                StudentID = studentModel.StudentID,
                Name = studentModel.Name,
                Age = studentModel.Age,
                Address = studentModel.Address,
                GPA = studentModel.GPA,
                StudentSubjectGrades = studentModel.StudentSubjectGrades != null
            ? studentModel.StudentSubjectGrades
                .Select(ssg => new StudentSubjectGradeDTO
                {
                    TeacherSubjectID = ssg.TeacherSubjectID,
                    SubjectName = ssg.TeacherSubject.Subject?.Name ?? "Unknown Subject" // Handle null Subject gracefully
                })
                .ToList()
            : new List<StudentSubjectGradeDTO>() // Handle null StudentSubjectGrades gracefully
            };
        }


        public static List<StudentSubjectTeacherGradeDTO> ToStudentsSubjectOnlyDTO(this Student studentModel)
        {
            return studentModel.StudentSubjectGrades.Select(ssg => new StudentSubjectTeacherGradeDTO
            {

                SubjectName = ssg.TeacherSubject.Subject.Name,
                TeacherName = ssg.TeacherSubject.Teacher.Name,
                GradeNumber = ssg.GradeNumber,

            }).ToList();
        }

        public static List<StudentSubjectGradeDTO> ToStudentsSubjectGradesOnlyDTO(this Student studentModel)
        {
            return studentModel.StudentSubjectGrades.Select(ssg => new StudentSubjectGradeDTO
            {
                TeacherSubjectID = ssg.TeacherSubjectID,
                SubjectName = ssg.TeacherSubject.Subject.Name,
                GradeNumber = ssg.GradeNumber
            }).ToList();
        }

        public static Student ToStudentFromCreateDTO(this CreateStudentRequestDTO studentDTO, List<TeacherSubject> targetSubjects)
        {
            var student = new Student
            {
                Name = studentDTO.Name,
                Age = studentDTO.Age,
                Address = studentDTO.Address,
                GPA = studentDTO.GPA,
                StudentSubjectGrades = new List<StudentSubjectGrade>()
                // You can add any default or calculated values for the Student if required
            };

            // Map StudentSubjectGrades
            foreach (var ssgDTO in studentDTO.StudentSubjectGrades)
            {
                var studentSubjectGrade = new StudentSubjectGrade
                {
                    TeacherSubjectID = ssgDTO.TeacherSubjectID, // Assign TeacherSubjectId based on the teacher-subject combination
                    GradeNumber = ssgDTO.GradeNumber,
                    Student = student,
                    TeacherSubject = targetSubjects.Find(ts => ts.TeacherSubjectID == ssgDTO.TeacherSubjectID)
                };

                student.StudentSubjectGrades.Add(studentSubjectGrade);
            }

            return student;
        }

        public static StudentSubjectGrade ToStudentSubject(this Student student, int? id, TeacherSubject subject)
        {
            return new StudentSubjectGrade
            {
                StudentID = student.StudentID,
                TeacherSubjectID = (int)id,
                Student = student,
                TeacherSubject = subject,
                GradeNumber = null

            };

        }
        public static TeacherSubjectDTO ToTeacherSubjectDTO(this TeacherSubject teacherSubjectModel)
        {
            return new TeacherSubjectDTO
            {
                TeacherSubjectID = teacherSubjectModel.TeacherSubjectID,
                TeacherName = teacherSubjectModel.Teacher.Name, 
                SubjectID = teacherSubjectModel.SubjectID,
                SubjectName = teacherSubjectModel.Subject.Name
            };
        }
    
        

    }






}

