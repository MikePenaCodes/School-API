using api.DTOs;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace api.Controllers
{
    [Route("api/teacher")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }
        //--------------------------------------------------------------------------------
        //Get All Teachers

        [HttpGet("get")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject2 query)
        {
            try
            {
                var teacherDTOs = await _teacherService.GetAllTeachersAsync(query);
                return Ok(teacherDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //--------------------------------------------------------------------------------
        //Get a Teacher by ID. 

        [HttpGet("getteacher")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var teacherDTOs = await _teacherService.GetTeacherByIdAsync(userId);
                return Ok(teacherDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //--------------------------------------------------------------------------------      
        //Create a new Teacher. 

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateTeacherRequestDTO teacherDTO)
        {
            try
            {
                var teacher = await _teacherService.CreateTeacherAsync(teacherDTO);
                return CreatedAtAction(nameof(GetById), new { teacherId = teacher.TeacherID }, teacher.ToTeacherDTO());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //--------------------------------------------------------------------------------
        //Get All Subjects.

        [HttpGet("subjects")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllSubjects()
        {
            try
            {
                var subjectDTOs = await _teacherService.GetSubjectsAsync();
                return Ok(subjectDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //--------------------------------------------------------------------------------
        //Get All Subject for a Teacher.

        [HttpGet("subjects/get")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSubjectsById()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var subjectDTOs = await _teacherService.GetSubjectsByTeacherIdAsync(userId);
                return Ok(subjectDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //--------------------------------------------------------------------------------
        //Get All Class Choices (Get All TeacherSubjects)
        [HttpGet("teachersubjects")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTeacherSubjectsbyteacherId()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var teachersubjects = await _teacherService.GetSubjectsByTeacherIdAsync(userId);
                return Ok(teachersubjects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //--------------------------------------------------------------------------------
        //Creates a Subject 

        [HttpPost("subjects/create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectDTO newSubjectDTO)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var subjectcreated = await _teacherService.CreateSubjectsByTeacherId(newSubjectDTO, userId);
                return CreatedAtAction(nameof(GetSubjectsById), new { userId = userId },
                new { Message = "The class was successfully created.", Data = subjectcreated });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //--------------------------------------------------------------------------------
        //Assigns a Subject to a Teacher

        [HttpPost("teachersubjects/assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTeacherSubject([FromQuery] QueryObject2 query)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var subject = await _teacherService.AssignSubjectToTeacher(query, userId);
                return CreatedAtAction(nameof(GetAllTeacherSubjectsbyteacherId), new { teacherId = query.TeacherId }, subject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //--------------------------------------------------------------------------------
        //Removes a Subject from a Teacher.

        [HttpDelete("teachersubjects/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTeacherSubject([FromQuery] QueryObject2 query)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await _teacherService.RemoveTeacherSubject(query, userId);
                return Ok(new { Message = "The class was successfully dropped." });
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException;
                if (innerException != null)
                {
                    return BadRequest(new { Message = "Database error while deleting the class. This class still contains students. Please make sure the class is empty before deleting it.", ErrorDetails = innerException.Message });
                }
                return BadRequest(new { Message = "An error occurred while deleting the class.", ErrorDetails = ex.Message });
            }
        }

        //--------------------------------------------------------------------------------
        //Delete a subject.

        [HttpDelete("subjects/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSubject([FromQuery] QueryObject2 query)
        {
            try
            {
                var result = await _teacherService.DeleteSubjectAsync(query.SubjectId);
                return Ok(new { Message = "The subject was successfully deleted." });
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException;
                if (innerException != null)
                {
                    return BadRequest(new { Message = "Database error while deleting the subject. This subject is still being taught. Please make sure there are no classes with this subject before deleting it.", ErrorDetails = innerException.Message });
                }
                return BadRequest(new { Message = "An error occurred while deleting the subject.", ErrorDetails = ex.Message });
            }
        }

        //--------------------------------------------------------------------------------
        //Get all Students in a Subject for a Teacher

        [HttpGet("subjects/students/get")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStudentsBySubject([FromQuery] QueryObject2 query)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var teachersubject = await _teacherService.GetAllStudentsBySubject(query, userId);
                return Ok(teachersubject.ToStudentsNamesOnlyDTO());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // //--------------------------------------------------------------------------------
        // //Update a Student's grade. 

        [HttpPut("updategrade")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] UpgradeStudentSubjectGradeDTO updateDTO)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var updatedStudentGrade = await _teacherService.UpdateGrade(updateDTO, userId);
                return Ok(updatedStudentGrade);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////
        //Get All Students

        [HttpGet("getallstudents")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAsync([FromQuery] QueryObject query)
        {
            try
            {
                var allStudent = await _teacherService.GetAllStudentsAsync(query);
                return Ok(allStudent); // Return the DTOs
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); ;
            }
        }

    }

}

