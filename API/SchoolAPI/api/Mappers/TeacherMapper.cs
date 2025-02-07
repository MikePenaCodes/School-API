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
                SubjectID = subjectModel.SubjectID,
                Name = subjectModel.Name,
            };
        }

        public static StudentNamesDTO ToStudentsNamesOnlyDTO(this TeacherSubject teacherSubject)
        {
            var studentNames = teacherSubject.StudentSubjectGrades
            .Select(ssg => new StudentNameGradeDTO
            {
                StudentID = ssg.StudentID,
                Name = ssg.Student.Name,
                GradeNumber = ssg.GradeNumber
            })
            .ToList();

            return new StudentNamesDTO
            {
                SubjectID = teacherSubject.TeacherSubjectID,
                SubjectName = teacherSubject.Subject.Name,
                Students = studentNames
            };
        }

        public static StudentSubjectIDGradeDTO UpdateStudentsGrade(this Teacher teacherModel, int teachersubjectid, int studentid, int? newGrade)
        {
            var studentGrade = teacherModel.TeacherSubjects
        .Where(ts => ts.TeacherSubjectID == teachersubjectid)
        .SelectMany(ts => ts.StudentSubjectGrades)
        .FirstOrDefault(ssg => ssg.StudentID == studentid);

            if (studentGrade == null)
            {
                return null;
                //Exception
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
                TeacherSubjectID = ts.TeacherSubjectID,
                SubjectID = ts.SubjectID,
                SubjectName = ts.Subject.Name,
            }).ToList();

        }

        public static Teacher ToTeacherFromCreateDTO(this CreateTeacherRequestDTO teacherDTO)
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
                var TeacherSubject = new TeacherSubject
                {
                    TeacherID = teacher.TeacherID,
                    SubjectID = ts.SubjectID
                };
                teacher.TeacherSubjects.Add(TeacherSubject);

            }
            ;
            return teacher;

        }

        public static Subject ToSubjectFromCreateDTO(this CreateSubjectDTO newsubjectDTO)
        {
            var subject = new Subject
            {
                Name = newsubjectDTO.Name,
                Hours = newsubjectDTO.Hours,
                // TeacherSubjects = new List<TeacherSubject>()
            };

            return subject;
        }
    }

}