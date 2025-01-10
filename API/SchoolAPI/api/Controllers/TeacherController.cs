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
    [Route("teachers")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public TeacherController(ApplicationDBContext context)
        {
            _context = context;
        }

        //////////////////////////////////////////////////////////////////////////////////
        //Get All Teacher

        [HttpGet("get")]
        public async Task<IActionResult> GetAllAsync()
        {
            var teachers = await _context.Teacher
            .Include(t => t.TeacherSubjects)
            .ThenInclude(ts => ts.Subject)
            .ToListAsync();

            var teacherDTOs = teachers.Select(t => t.ToTeacherDTO()).ToList();

            return Ok(teacherDTOs);
        }

        //////////////////////////////////////////////////////////////////////////////////
        //Get a Teacher by ID. 

        [HttpGet("get/{id}")]
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

        ////////////////////////////////////////////////////////////////////////////////        
        //Create a new Teacher. 

        [HttpPost("create")]
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

        ///////////////////////////////////////////////////////////////////////////////////
        //Get All Subjects.

        [HttpGet("subjects/get")]
        public async Task<IActionResult> GetAllSubjectsAsync()
        {
            var subjects = await _context.Subject
            .ToListAsync();

            var subjectDTOs = subjects.Select(s => s.ToSubjectDTO()).ToList();

            return Ok(subjectDTOs);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        //Get All Subject for a Teacher.

        [HttpGet("subjects/get/{id}")]
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

        /////////////////////////////////////////////////////////////////////////////////////
        //Creates and assigns a Subject to a Teacher. 

        [HttpPost("subjects/create")]
        public IActionResult CreateTeacherSubject([FromBody] CreateSubjectDTO newSubjectDTO)
        {
            if (newSubjectDTO == null)
            {
                return BadRequest("Subject data is required");
            }

            var teacher = _context.Teacher.FirstOrDefault(t => t.TeacherID == newSubjectDTO.TeacherID);
            if (teacher == null)
            {
                return BadRequest($"Teacher with ID {newSubjectDTO.TeacherID} not found");
            }

            var newSubject = newSubjectDTO.ToSubjectFromCreateDTO(_context);

            _context.Subject.Add(newSubject);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetSubjectsById), new { id = teacher.TeacherID }, newSubject);

        }

        /////////////////////////////////////////////////////////////////////////////////////
        //Removes a Subject from a Teacher.

        [HttpDelete("{teacherId}/subjects/delete/{subjectId}")]
        public async Task<IActionResult> Delete([FromRoute] int subjectId, int teacherId)
        {
            var teacherSubject = _context.TeacherSubject.FirstOrDefault(ssg => ssg.SubjectID == subjectId & ssg.TeacherID == teacherId);

            if (teacherSubject == null)
            {
                return NotFound();
            }

            _context.TeacherSubject.Remove(teacherSubject);

            _context.SaveChanges();

            return NoContent();
        }

        //////////////////////////////////////////////////////////////////////////////////////
        //Delete a subject.

        [HttpDelete("subjects/delete/{subjectId}")]
        public async Task<IActionResult> Delete([FromRoute] int subjectId)
        {
            var subject = _context.Subject.FirstOrDefault(s => s.SubjectID == subjectId);

            if (subject == null)
            {
                return NotFound();
            }

            _context.Subject.Remove(subject);

            _context.SaveChanges();

            return NoContent();
        }

        /////////////////////////////////////////////////////////////////////////////////////////
        //Get all Students in a Subject for a Teacher

        [HttpGet("{teacherId}/subjects/{subjectId}/students/get")]
        public async Task<IActionResult> GetStudentsBySubject([FromRoute] int teacherId, int subjectId)
        {
            var teacher = await _context.Teacher
            .Include(t => t.TeacherSubjects)
            .ThenInclude(ts => ts.Subject)
            .ThenInclude(s => s.StudentSubjectGrades)
            .ThenInclude(ssg => ssg.Student)
            .Where(t => t.TeacherID == teacherId)
            //.Where(t => t.TeacherSubjects.Any(ts => ts.Subject.SubjectID == subjectId))
            .FirstOrDefaultAsync();

            if (teacher == null)
            {
                return NotFound("Teacher not found or does not teach the specified subject.");
            }

            return Ok(teacher.ToStudentsNamesOnlyDTO(subjectId));
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        //Update a Student's grade. 

        [HttpPut("{teacherId}/subjects/{subjectId}/students/update/{studentId}")]
        public async Task<IActionResult> Update([FromRoute] int teacherId, int subjectId, int studentId, [FromBody] StudentSubjectIDGradeDTO updateDTO)
        {
            var teacher = await _context.Teacher
                .Include(t => t.TeacherSubjects)
                .ThenInclude(ts => ts.Subject)
                .ThenInclude(s => s.StudentSubjectGrades)
                .ThenInclude(ssg => ssg.Student)
                .Where(t => t.TeacherID == teacherId)
                //.Where(t => t.TeacherSubjects.Any(ts => ts.Subject.SubjectID == subjectId))
                .FirstOrDefaultAsync();

            if (teacher == null)
            {
                return NotFound("Teacher not found or does not teach the specified subject.");
            }

            var updatedStudentGrade = teacher.UpdateStudentsGrade(subjectId, studentId, updateDTO.GradeNumber);

            if (updatedStudentGrade == null)
            {
                return NotFound("Student or subject not found.");
            }

            await _context.SaveChangesAsync();

            return Ok(updatedStudentGrade);

        }










    }

}

