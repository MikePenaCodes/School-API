using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Mappers;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;

namespace api.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : Controller
    {
        private readonly ApplicationDBContext _context;

        public StudentController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var students = await _context.Student
                .Include(s => s.StudentSubjectGrades)
                .ThenInclude(ssg => ssg.Subject)
                .ToListAsync(); // Fetch the students with their related data

            var studentDtos = students.Select(s => s.ToStudentDTO()).ToList(); // Convert the students to DTOs

            return Ok(studentDtos); // Return the DTOs
        }


        //IActionResult = wrapper to return api type.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var student = await _context.Student
                .Include(s => s.StudentSubjectGrades)
                .ThenInclude(ssg => ssg.Subject)
                .FirstOrDefaultAsync(s => s.StudentID == id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student.ToStudentDTO());
        }

        [HttpGet("{id}/subjects")]
        public async Task<IActionResult> GetSubjectsById([FromRoute] int id)
        {
            var student = await _context.Student
                .Include(s => s.StudentSubjectGrades)
                .ThenInclude(ssg => ssg.Subject)
                .FirstOrDefaultAsync(s => s.StudentID == id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student.ToStudentsSubjectOnlyDTO());
        }


        //Current
        [HttpPost("{id}/subjects")]
        public async Task<IActionResult> Create(int id, [FromBody] EnrollinSubjectDTO newsubjectDTO)
        {
            if (newsubjectDTO == null)
            {
                return BadRequest("SubjectID is required.");
            }

            var studentsubject = await _context.Student.Include(s => s.StudentSubjectGrades).ThenInclude(ssg => ssg.Subject)
            .FirstOrDefaultAsync(s => s.StudentID == id);

            var subject = _context.Subject.FirstOrDefault(s => s.SubjectID == newsubjectDTO.SubjectID);

            if (studentsubject == null)
            {
                return NotFound();
            };

            var newsubject = studentsubject.ToSubjectFromCreateDTO(_context, newsubjectDTO, subject);


            _context.StudentSubjectGrade.Add(newsubject);
            _context.SaveChanges();

            // Return the created student as a DTO
            return CreatedAtAction(nameof(GetById), new { id = studentsubject.StudentID }, studentsubject.ToStudentnogradeDTO());
        }

        [HttpDelete("{studentid}/subjects/{subjectid}")]
        public async Task<IActionResult> Delete([FromRoute] int subjectid, int studentid)
        {
            var studentsubject = _context.StudentSubjectGrade.FirstOrDefault(ssg => ssg.SubjectID == subjectid & ssg.StudentID == studentid);

            if (studentsubject == null)
            {
                return NotFound();
            }

            _context.StudentSubjectGrade.Remove(studentsubject);

            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("{id}/subjectsgrades")]
        public async Task<IActionResult> GetSubjectGradesById([FromRoute] int id)
        {
            var student = await _context.Student
                .Include(s => s.StudentSubjectGrades)
                .ThenInclude(ssg => ssg.Subject)
                .FirstOrDefaultAsync(s => s.StudentID == id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student.ToStudentsSubjectGradesOnlyDTO());
        }



        [HttpPost]
        public IActionResult Create([FromBody] CreateStudentRequestDTO studentDTO)
        {
            if (studentDTO == null)
            {
                return BadRequest("Student data is required.");
            }

            // Check if all subjects exist in the database and retrieve them
            foreach (var ssg in studentDTO.StudentSubjectGrades)
            {
                var subject = _context.Subject.FirstOrDefault(s => s.SubjectID == ssg.SubjectID);
                if (subject == null)
                {
                    return BadRequest($"Subject with ID {ssg.SubjectID} not found.");
                }

                // Assign the SubjectName (to be returned later in the DTO)
                ssg.SubjectName = subject.Name;
            }

            // Map the StudentDTO to a Student entity (including StudentSubjectGrades)
            var student = studentDTO.ToStudentFromCreateDTO(_context);

            // Add the student to the database
            _context.Student.Add(student);
            _context.SaveChanges();

            // Return the created student as a DTO
            return CreatedAtAction(nameof(GetById), new { id = student.StudentID }, student.ToStudentDTO());
        }

    }

}