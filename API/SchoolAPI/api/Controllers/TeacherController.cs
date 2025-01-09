using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using api.DTOs;
using api.Mappers;
using Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TextTemplating;
using Models;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;

namespace api.Controllers
{
    [Route("api/teacher")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public TeacherController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var teachers = await _context.Teacher
            .Include(t => t.TeacherSubjects)
            .ThenInclude(ts => ts.Subject)
            .ToListAsync();

            var teacherDTOs = teachers.Select(t => t.ToTeacherDTO()).ToList();

            return Ok(teacherDTOs);
        }

        [HttpGet("subjects")]
        public async Task<IActionResult> GetAllSubjectsAsync()
        {
            var subjects = await _context.Subject
            .ToListAsync();

            var subjectDTOs = subjects.Select(s => s.ToSubjectDTO()).ToList();

            return Ok(subjectDTOs);
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var teacher = await _context.Teacher
            .Include(t => t.TeacherSubjects)
            .ThenInclude(ts => ts.Subject)
            .FirstOrDefaultAsync(t => t.TeacherID == id);

            if (teacher == null)
            {
                return NotFound();
            }
            return Ok(teacher.ToTeacherDTO());
        }

        [HttpGet("{id}/subjects")]
        public async Task<IActionResult> GetSubjectsById([FromRoute] int id)
        {
            var teacher = await _context.Teacher
            .Include(t => t.TeacherSubjects)
            .ThenInclude(ts => ts.Subject)
            .FirstOrDefaultAsync(t => t.TeacherID == id);

            if (teacher == null)
            {
                return NotFound();
            }
            return Ok(teacher.ToTeacherSubjectsOnlyDTO());
        }

        [HttpGet("{teacherid}/subjects/{subjectid}/students")]
        public async Task<IActionResult> GetStudentBySubject([FromRoute] int teacherid, int subjectid)
        {
            var teacher = await _context.Teacher
            .Include(t => t.TeacherSubjects)
            .ThenInclude(ts => ts.Subject)
            .ThenInclude(s => s.StudentSubjectGrades)
            .ThenInclude(ssg => ssg.Student)
            .Where(t => t.TeacherID == teacherid)
            //.Where(t => t.TeacherSubjects.Any(ts => ts.Subject.SubjectID == subjectid))
            .FirstOrDefaultAsync();

            if (teacher == null)
            {
                return NotFound("Teacher not found or does not teach the specified subject.");
            }

            return Ok(teacher.ToStudentsNamesOnlyDTO(subjectid));
        }

        [HttpPut("{teacherid}/subjects/{subjectid}/students/{studentid}")]
        public async Task<IActionResult> Update([FromRoute] int teacherid, int subjectid, int studentid, [FromBody] StudentSubjectIdGradeDTO updateDTO)
        {
            var teacher = await _context.Teacher
                .Include(t => t.TeacherSubjects)
                .ThenInclude(ts => ts.Subject)
                .ThenInclude(s => s.StudentSubjectGrades)
                .ThenInclude(ssg => ssg.Student)
                .Where(t => t.TeacherID == teacherid)
                //.Where(t => t.TeacherSubjects.Any(ts => ts.Subject.SubjectID == subjectid))
                .FirstOrDefaultAsync();

            if (teacher == null)
            {
                return NotFound("Teacher not found or does not teach the specified subject.");
            }

            var updatedStudentGrade = teacher.UpdateStudentsGrade(subjectid, studentid, updateDTO.GradeNumber);

            if (updatedStudentGrade == null)
            {
                return NotFound("Student or subject not found.");
            }

            await _context.SaveChangesAsync();

            return Ok(updatedStudentGrade);

        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateTeacherRequestDTO teacherDTO)
        {
            if (teacherDTO == null)
            {
                return BadRequest("Teacher data is required.");
            }

            foreach (var ts in teacherDTO.TeacherSubjects)
            {
                var subject = _context.Subject.FirstOrDefault(t => t.SubjectID == ts.SubjectID);
                if (subject == null)
                {
                    return BadRequest($"Subject with ID {ts.SubjectID} not found");
                }

                ts.SubjectName = subject.Name;
            }
            var teacher = teacherDTO.ToTeacherFromCreateDTO(_context);

            _context.Teacher.Add(teacher);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = teacher.TeacherID }, teacher.ToTeacherDTO());
        }

        //Create a Teacher Subject item. TeacherID, SubjectID, Teacher, Subject
        [HttpPost("subjects")]
        public IActionResult CreateTeacherSubject([FromBody] CreateSubjectDTO newsubjectDTO)
        {
            if (newsubjectDTO == null)
            {
                return BadRequest("Subject data is required");
            }

            var teacher = _context.Teacher.FirstOrDefault(t => t.TeacherID == newsubjectDTO.TeacherID);
            if (teacher == null)
            {
                return BadRequest($"Teacher with ID {newsubjectDTO.TeacherID} not found");
            }

            var newsubject = newsubjectDTO.ToSubjectFromCreateDTO(_context);

            _context.Subject.Add(newsubject);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetSubjectsById), new { id = teacher.TeacherID }, newsubject);

        }

        [HttpDelete("{teacherid}/subjects/{subjectid}")]
        public async Task<IActionResult> Delete([FromRoute] int subjectid, int teacherid)
        {
            var teachersubject = _context.TeacherSubject.FirstOrDefault(ssg => ssg.SubjectID == subjectid & ssg.TeacherID == teacherid);

            if (teachersubject == null)
            {
                return NotFound();
            }

            _context.TeacherSubject.Remove(teachersubject);

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("subjects/{subjectid}")]
        public async Task<IActionResult> Delete([FromRoute] int subjectid)
        {
            var subject = _context.Subject.FirstOrDefault(s => s.SubjectID == subjectid);

            if (subject == null)
            {
                return NotFound();
            }

            _context.Subject.Remove(subject);

            _context.SaveChanges();

            return NoContent();
        }



    }

}

