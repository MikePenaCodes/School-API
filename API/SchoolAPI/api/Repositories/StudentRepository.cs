using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.ApplicationLayer;
using api.Interfaces;
using api.Models;
using Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace api.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDBContext _context;

        public StudentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetPaginatedStudentsAsync(int skip, int pageSize)
        {
            var students = await _context.Student
                .Include(s => s.StudentSubjectGrades)
                .ThenInclude(ssg => ssg.TeacherSubject)
                .ThenInclude(ts => ts.Subject)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            if (students == null)
            {
                throw new KeyNotFoundException($"No students enrolled.");
            }
            return students;
        }

        public async Task<Student> GetStudentbyId(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "StudentId is required.");
            }

            var targetStudent = await _context.Student
                .Include(s => s.StudentSubjectGrades)
                .ThenInclude(ssg => ssg.TeacherSubject)
                .ThenInclude(ts => ts.Subject)
                .Include(s => s.StudentSubjectGrades)
                .ThenInclude(ssg => ssg.TeacherSubject)
                .ThenInclude(ts => ts.Teacher)
                .FirstOrDefaultAsync(s => s.UserId == id);

            if (targetStudent == null)
            {
                throw new KeyNotFoundException($"Student with ID {id} not found");
            }
            return targetStudent;
        }

        public async Task RemoveStudentSubject(StudentSubjectGrade studentSubject)
        {
            _context.StudentSubjectGrades.Remove(studentSubject);

            await _context.SaveChangesAsync();
        }

        public async Task<TeacherSubject> GetTeacherSubjects(int? id)
        {
             if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "SubjectId is required.");
            }
            var targetSubject = await _context.TeacherSubjects.Include(s => s.Subject).FirstOrDefaultAsync(s => s.TeacherSubjectID == id);
            if (targetSubject == null)
            {
                throw new KeyNotFoundException($"TeacherSubject with ID {id} not found");
            }

            return targetSubject;
        }
        
        public async Task<Student> CreateStudent(Student student)
        {
            _context.Student.Add(student);
            await _context.SaveChangesAsync();

            return student;

        }
        public async Task CreateStudentSubjectGrade(StudentSubjectGrade studentSubject)
        {
            _context.StudentSubjectGrades.Add(studentSubject);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TeacherSubject>> GetPaginatedTeacherSubjectsAsync(int skip, int pageSize)
        {
            var teachersubjects = await _context.TeacherSubjects
                .Include(s =>s.Subject)
                .Include(s =>s.Teacher)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            if (teachersubjects == null)
            {
                throw new KeyNotFoundException($"No Teacher Subjects.");
            }
            return teachersubjects;
        }
    }



}