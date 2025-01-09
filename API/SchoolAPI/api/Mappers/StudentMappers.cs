using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
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
                    SubjectID = ssg.SubjectID,
                    SubjectName = ssg.Subject.Name,
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
                    SubjectID = ssg.SubjectID,
                    SubjectName = ssg.Subject?.Name ?? "Unknown Subject" // Handle null Subject gracefully
                })
                .ToList()
            : new List<StudentSubjectGradeDTO>() // Handle null StudentSubjectGrades gracefully
            };
        }


        public static List<StudentSubjectDTO> ToStudentsSubjectOnlyDTO(this Student studentModel)
        {
            return studentModel.StudentSubjectGrades.Select(ssg => new StudentSubjectDTO
            {
                SubjectID = ssg.SubjectID,
                SubjectName = ssg.Subject.Name,
            }).ToList();
        }

        public static List<StudentSubjectGradeDTO> ToStudentsSubjectGradesOnlyDTO(this Student studentModel)
        {
            return studentModel.StudentSubjectGrades.Select(ssg => new StudentSubjectGradeDTO
            {
                SubjectID = ssg.SubjectID,
                SubjectName = ssg.Subject.Name,
                GradeNumber = ssg.GradeNumber
            }).ToList();
        }

        public static Student ToStudentFromCreateDTO(this CreateStudentRequestDTO studentDTO, ApplicationDBContext context)
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
                var subject = context.Subject.FirstOrDefault(s => s.SubjectID == ssgDTO.SubjectID);
                if (subject != null)
                {
                    var studentSubjectGrade = new StudentSubjectGrade
                    {
                        SubjectID = ssgDTO.SubjectID,
                        Subject = subject, // Link the subject
                        GradeNumber = ssgDTO.GradeNumber,
                        Student = student
                    };
                    student.StudentSubjectGrades.Add(studentSubjectGrade);
                }
                else
                {
                    // Optionally, handle the case where the subject was not found, maybe throw an exception
                    // or return an error if needed
                    // Example: throw new Exception($"Subject with ID {ssgDTO.SubjectID} not found.");
                }
            }

            return student;
        }



        public static StudentSubjectGrade ToSubjectFromCreateDTO(this Student student, ApplicationDBContext context, EnrollinSubjectDTO newsubjectDTO, Subject subject)
        {
            return new StudentSubjectGrade
            {
                StudentID = student.StudentID,
                SubjectID = newsubjectDTO.SubjectID,
                Student = student,
                Subject = subject

            };

        }
    }






}

