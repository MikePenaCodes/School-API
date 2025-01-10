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
    public static class TeacherMappers
    {
        public static TeacherDTO ToTeacherDTO(this Teacher teacherModel)
        {
            return new TeacherDTO
            {
                TeacherID = teacherModel.TeacherID,
                Name = teacherModel.Name,
                Age = teacherModel.Age,
                Address = teacherModel.Address,
                YearsofExp = teacherModel.YearsofExp,
                TeacherSubjects = teacherModel.TeacherSubjects.Select(ts => new TeacherSubjectDTO
                {
                    SubjectID = ts.SubjectID,
                    SubjectName = ts.Subject.Name,
                }).ToList()
            };
        }

        public static SubjectDTO ToSubjectDTO(this Subject subjectModel)
        {
            return new SubjectDTO
            {
                Name = subjectModel.Name,
                TeacherID = subjectModel.TeacherID,
                StudentSubjectGrades = new List<StudentSubjectGrade>(),
                TeacherSubjects = new List<TeacherSubject>()
            .ToList()
            };

        }

        public static List<StudentNamesDTO> ToStudentsNamesOnlyDTO(this Teacher teacherModel, int subjectid)
        {
            return teacherModel.TeacherSubjects
            .Where(ts => ts.SubjectID == subjectid)
        .SelectMany(ts => ts.Subject.StudentSubjectGrades)
        .Select(ssg => new StudentNamesDTO
        {
            StudentID = ssg.Student.StudentID,
            Name = ssg.Student.Name,
        }).ToList();

        }
        public static StudentSubjectIDGradeDTO UpdateStudentsGrade(this Teacher teacherModel, int subjectid, int studentid, int? newGrade)
        {
            var studentGrade = teacherModel.TeacherSubjects
        .Where(ts => ts.SubjectID == subjectid)
        .SelectMany(ts => ts.Subject.StudentSubjectGrades)
        .FirstOrDefault(ssg => ssg.StudentID == studentid);

            if (studentGrade == null)
            {
                return null;
            }

            studentGrade.GradeNumber = newGrade;

            return new StudentSubjectIDGradeDTO
            {
                StudentID = studentGrade.StudentID,
                StudentName = studentGrade.Student.Name,
                GradeNumber = studentGrade.GradeNumber
            };
        }

        public static List<TeacherSubjectDTO> ToTeacherSubjectsOnlyDTO(this Teacher teacherModel)
        {
            return teacherModel.TeacherSubjects.Select(ts => new TeacherSubjectDTO
            {
                SubjectID = ts.SubjectID,
                SubjectName = ts.Subject.Name,
            }).ToList();

        }
        public static Teacher ToTeacherFromCreateDTO(this CreateTeacherRequestDTO teacherDTO, ApplicationDBContext context)
        {
            var teacher = new Teacher
            {
                Name = teacherDTO.Name,
                Age = teacherDTO.Age,
                Address = teacherDTO.Address,
                YearsofExp = teacherDTO.YearsofExp,
                TeacherSubjects = new List<TeacherSubject>()
            };

            foreach (var ts in teacherDTO.TeacherSubjects)
            {
                var subject = context.Subject.FirstOrDefault(s => s.SubjectID == ts.SubjectID);
                if (subject != null)
                {
                    var TeacherSubject = new TeacherSubject
                    {
                        TeacherID = ts.SubjectID,
                        Subject = subject
                    };
                    teacher.TeacherSubjects.Add(TeacherSubject);
                }
                else { }
            }
            return teacher;

        }

        public static Subject ToSubjectFromCreateDTO(this CreateSubjectDTO newsubjectDTO, ApplicationDBContext context)
        {
            var subject = new Subject
            {
                Name = newsubjectDTO.Name,
                TeacherID = newsubjectDTO.TeacherID,
                StudentSubjectGrades = new List<StudentSubjectGrade>(),
                TeacherSubjects = new List<TeacherSubject>()
            };

            var teacher = context.Teacher.FirstOrDefault(s => s.TeacherID == subject.TeacherID);
            if (teacher != null)
            {
                var teacherSubject = new TeacherSubject
                {
                    TeacherID = subject.TeacherID,
                    SubjectID = subject.SubjectID,
                    Teacher = teacher,
                    Subject = subject

                };


                subject.TeacherSubjects.Add(teacherSubject);
            }


            return subject;
        }
    }

}