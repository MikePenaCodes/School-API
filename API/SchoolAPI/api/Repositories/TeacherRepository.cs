using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace api.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDBContext _context;

        public TeacherRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<Teacher>> GetPaginatedTeachersAsync(int skip, int pageSize)
        {
            var teachers = await _context.Teacher
               .Include(t => t.TeacherSubjects)
               .ThenInclude(ts => ts.Subject)
               .Skip(skip)
               .Take(pageSize)
               .ToListAsync();

            if (teachers == null)
            {
                throw new KeyNotFoundException($"No teachers hired.");
            }
            return teachers;
        }

        public async Task<Teacher> GetTeacherById(string id)
        {

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "TeacherId is required.");
            }

            var teacherfound  = await _context.Teacher
                .Include(t => t.TeacherSubjects)
                .ThenInclude(ts => ts.Subject)
                .FirstOrDefaultAsync(t => t.UserId == id);
            if (teacherfound == null)
            {
                throw new KeyNotFoundException($"Teacher with ID {id} not found");
            }

            return teacherfound;
        }
        public async Task<Subject> GetSubject(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "SubjectId is required.");
            }
            var subject = await _context.Subject.FirstOrDefaultAsync(s => s.SubjectID == id);
            if (subject == null)
            {
             throw new KeyNotFoundException($"Subject with ID {id} not found");
            }
            return subject;
        }
        public async Task<List<Subject>> GetSubjects()
        {

            var subject = await _context.Subject.ToListAsync();
            if (subject == null)
            {
             throw new KeyNotFoundException($"No Subjects Found");
            }
            return subject;
        }
        public async Task<TeacherSubject> GetTeacherSubjects(int? id)
        {
            var targetSubject = await _context.TeacherSubjects.Include(s => s.Subject).FirstOrDefaultAsync(s => s.TeacherSubjectID == id);
            if (targetSubject == null)
                {
                    throw new KeyNotFoundException($"TeacherSubject with ID {id} not found");
                }

            return targetSubject;
        }

            public async Task<Teacher>CreateTeacher(Teacher teacher)
            {
                _context.Teacher.Add(teacher);
                await _context.SaveChangesAsync();

                return teacher;
            }

            public async Task<Subject> CreateSubject(Subject subject)
            {
                _context.Subject.Add(subject);
                await _context.SaveChangesAsync();

                return subject;
            }

            public async Task<TeacherSubject> CreateTeacherSubject(TeacherSubject teacherSubject)
            {
                _context.TeacherSubjects.Add(teacherSubject);
                await _context.SaveChangesAsync();

                return teacherSubject;
            }

            
            public async Task<TeacherSubject> DeleteTeacherSubject(TeacherSubject teacherSubject)
            {
                _context.TeacherSubjects.Remove(teacherSubject);

                await _context.SaveChangesAsync();

                return teacherSubject;


            }
            public async Task<Subject> DeleteSubject(Subject subject)
            {
            _context.Subject.Remove(subject);
            await _context.SaveChangesAsync();

            return subject;
            }

            public async Task<Teacher> GetAllTables(string id)
            {
                var teacher = await _context.Teacher
                    .Include(t => t.TeacherSubjects)
                    .ThenInclude(s => s.StudentSubjectGrades)
                    .ThenInclude(ssg => ssg.Student)
                    .Include(t => t.TeacherSubjects)
                    .ThenInclude(s => s.Subject)
                    .Where(t => t.UserId == id)
                    .FirstOrDefaultAsync();
            if (teacher == null)
            {
                throw new KeyNotFoundException($"Teacher with ID {id} not found");
            }

            return teacher;

            }

            public async Task UpdateGrade()
            {
                await _context.SaveChangesAsync();
            }


        
    
}
}