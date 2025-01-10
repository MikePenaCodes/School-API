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

        //////////////////////////////////////////////////////////////////////////////////
        //Get All Students

        [HttpGet("get")]
        public async Task<IActionResult> GetAllAsync()
        {
            var allStudents = await _context.Student
                .Include(s => s.StudentSubjectGrades)
                .ThenInclude(ssg => ssg.Subject)
                .ToListAsync(); // Fetch the students with their related data

            var studentDtos = allStudents.Select(s => s.ToStudentDTO()).ToList(); // Convert the students to DTOs

            return Ok(studentDtos); // Return the DTOs
        }

        //////////////////////////////////////////////////////////////////////////////////
        //Get Student by Id


        //IActionResult = wrapper to return api type.
        [HttpGet("get/{studentId}")]
        public async Task<IActionResult> GetById([FromRoute] int studentId)
        {
            var targetStudent = await _context.Student
                .Include(s => s.StudentSubjectGrades)
                .ThenInclude(ssg => ssg.Subject)
                .FirstOrDefaultAsync(s => s.StudentID == studentId);

            if (targetStudent == null)
            {
                return NotFound();
            }

            return Ok(targetStudent.ToStudentDTO());
        }

        //////////////////////////////////////////////////////////////////////////////////
        //Create a student

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateStudentRequestDTO studentDTO)
        {
            if (studentDTO == null)
            {
                return BadRequest("Student data is required.");
            }

            // Check if all subjects exist in the database and retrieve them
            foreach (var ssg in studentDTO.StudentSubjectGrades)
            {
                var targetSubject = _context.Subject.FirstOrDefault(s => s.SubjectID == ssg.SubjectID);
                if (targetSubject == null)
                {
                    return BadRequest($"Subject with ID {ssg.SubjectID} not found.");
                }

                // Assign the SubjectName (to be returned later in the DTO)
                ssg.SubjectName = targetSubject.Name;
            }

            // Map the StudentDTO to a Student entity (including StudentSubjectGrades)
            var student = studentDTO.ToStudentFromCreateDTO(_context);

            // Add the student to the database
            _context.Student.Add(student);
            _context.SaveChanges();

            // Return the created student as a DTO
            return CreatedAtAction(nameof(GetById), new { studentId = student.StudentID }, student.ToStudentDTO());
        }

        //////////////////////////////////////////////////////////////////////////////////
        //Get Subjects by StudentId

        [HttpGet("{studentId}/subjects/get")]
        public async Task<IActionResult> GetSubjectsById([FromRoute] int studentId)
        {
            var student = await _context.Student
                .Include(s => s.StudentSubjectGrades)
                .ThenInclude(ssg => ssg.Subject)
                .FirstOrDefaultAsync(s => s.StudentID == studentId);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student.ToStudentsSubjectOnlyDTO());
        }

        //////////////////////////////////////////////////////////////////////////////////
        //Enroll student in a subject

        [HttpPost("{studentId}/subjects/create")]
        public async Task<IActionResult> Create(int studentId, [FromBody] EnrollinSubjectDTO subjectToEnrollDTO)
        {
            if (subjectToEnrollDTO == null)
            {
                return BadRequest("SubjectID is required.");
            }

            var studentSubject = await _context.Student.Include(s => s.StudentSubjectGrades).ThenInclude(ssg => ssg.Subject)
            .FirstOrDefaultAsync(s => s.StudentID == studentId);

            var subjectFromDatabase = _context.Subject.FirstOrDefault(s => s.SubjectID == subjectToEnrollDTO.SubjectID);

            if (studentSubject == null)
            {
                return NotFound();
            };

            var subjectToEnroll = studentSubject.ToSubjectFromCreateDTO(_context, subjectToEnrollDTO, subjectFromDatabase);


            _context.StudentSubjectGrade.Add(subjectToEnroll);
            _context.SaveChanges();

            // Return the created student as a DTO
            return CreatedAtAction(nameof(GetById), new { studentId = studentSubject.StudentID }, studentSubject.ToStudentnogradeDTO());
        }

        //////////////////////////////////////////////////////////////////////////////////
        //Drop class 

        [HttpDelete("{studentId}/subjects/{subjectId}")]
        public async Task<IActionResult> Delete([FromRoute] int subjectId, int studentId)
        {
            var studentSubject = _context.StudentSubjectGrade.FirstOrDefault(ssg => ssg.SubjectID == subjectId & ssg.StudentID == studentId);

            if (studentSubject == null)
            {
                return NotFound();
            }

            _context.StudentSubjectGrade.Remove(studentSubject);

            _context.SaveChanges();

            return NoContent();
        }

        //////////////////////////////////////////////////////////////////////////////////
        //Get Grades from Student

        [HttpGet("{studentId}/subjectsgrades")]
        public async Task<IActionResult> GetSubjectGradesById([FromRoute] int studentId)
        {
            var targetStudent = await _context.Student
                .Include(s => s.StudentSubjectGrades)
                .ThenInclude(ssg => ssg.Subject)
                .FirstOrDefaultAsync(s => s.StudentID == studentId);

            if (targetStudent == null)
            {
                return NotFound();
            }

            return Ok(targetStudent.ToStudentsSubjectGradesOnlyDTO());
        }



    }

}