using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.DTOs;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Data;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }



        //////////////////////////////////////////////////////////////////////////////////
        //Get Student by Id

        [HttpGet("getstudent")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetById()
        {

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var targetStudent = await _studentService.GetStudentByIdAsync(userId);
                return Ok(targetStudent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        //////////////////////////////////////////////////////////////////////////////////
        //Create a student

        [HttpPost("create")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create([FromBody] CreateStudentRequestDTO studentDTO)
        {

            try
            {
                var student = await _studentService.CreateStudentAsync(studentDTO);
                return CreatedAtAction(nameof(GetById), new { studentId = student.StudentID }, student.ToStudentDTO());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        //////////////////////////////////////////////////////////////////////////////////
        //Get Subjects by StudentId

        [HttpGet("subjects/get")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetSubjectsById([FromQuery] QueryObject query)
        {

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var studentsubjects = await _studentService.GetSubjectsStudentIdAsync(query, userId);
                return Ok(studentsubjects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        //////////////////////////////////////////////////////////////////////////////////
        //Enroll student in a subject

        [HttpPost("subjects/enroll")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create([FromQuery] QueryObject query)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var studentSubject = await _studentService.EnrollSubject(query, userId);
                return CreatedAtAction(nameof(GetById), new { studentId = studentSubject.StudentID }, studentSubject.ToStudentnogradeDTO());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////
        //Drop class 

        [HttpDelete("subjects/drop")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Delete([FromQuery] QueryObject query)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var enrolledSubject = await _studentService.DropClass(query, userId);
                return Ok(new { Message = "The class was successfully dropped." });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //////////////////////////////////////////////////////////////////////////////////
        //Get Grades from Student

        [HttpGet("grades")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetSubjectGradesById([FromQuery] QueryObject query)
        {

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var students = await _studentService.GetStudentGradesByIdAsync(query, userId);
                return Ok(students);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("subjectoptions")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetTeacherSubjects([FromQuery] QueryObject query)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var students = await _studentService.GetTeacherSubjects(query);
                return Ok(students);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}