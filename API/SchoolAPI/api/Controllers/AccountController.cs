using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.DTOs;
using api.Interfaces;
using api.Models;
using Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Models;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using api.Services;


namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signinManager;
        private readonly IEmailService _emailService;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signinManager, ApplicationDBContext context, IEmailService emailService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signinManager;
            _context = context;
            _emailService = emailService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(loginDto.UserName) || string.IsNullOrEmpty(loginDto.Password))
                return BadRequest();


            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
            if (user == null) return Unauthorized("Invalid username!");
            bool isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed) return BadRequest("You need to confirm email before loggin in");



            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Username not found and/or password is incorrect");



            return Ok(
                new newUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = await _tokenService.CreateToken(user)
                }
            );
        }

    // need to input email and send email a code 
    // make a new endpoint with that can request code



        [HttpPost("forgot")]
        public async Task<IActionResult> Forgot([FromBody] ForgotPasswordDTO forgotPassswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var email = forgotPassswordDto.Email;
                if (email == null)
                    return BadRequest();
                var _user = await GetUser(email);
                if (_user == null) return NotFound();
                var emailCode = await _userManager.GeneratePasswordResetTokenAsync(_user!);
                var sendEmail = SendPasswordCodeEmail(_user!, emailCode);
                return Ok(sendEmail);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("forgot/confirmation")]
        public async Task<IActionResult> PasswordConfirmation([FromBody] NewPassWordDto newPassWordDto)
        {
            // if (string.IsNullOrEmpty(newPassWordDto.Email))
            //     return BadRequest("Invalid code provided");
            
            var user = await GetUser(newPassWordDto.Email);


            var result = await _userManager.ResetPasswordAsync(user, newPassWordDto.Code.ToString(), newPassWordDto.Password);
           
            if (!result.Succeeded)
                return BadRequest(result);
            else
                return Ok("Password reset successfully, you can proceed to login");

        }


    
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await GetUser(registerDto.Email);
                if (user != null) return BadRequest();


                var appUser = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                    if (roleResult.Succeeded)
                    {
                        var student = new Student
                        {
                            Name = registerDto.Name,
                            Age = registerDto.Age,
                            Address = registerDto.Address,
                            GPA = registerDto.GPA,
                            UserId = appUser.Id
                        };

                        _context.Student.Add(student);
                        await _context.SaveChangesAsync();


                        // return Ok(new newUserDto
                        // {
                        //     UserName = appUser.UserName,
                        //     Email = appUser.Email,
                        //     Token = await _tokenService.CreateToken(appUser)
                        // });
                    }

                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
                var _user = await GetUser(registerDto.Email);
                var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(_user!);
                var sendEmail = await SendEmail(_user!, emailCode);
                return Ok(sendEmail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        private async Task<string> SendPasswordCodeEmail(AppUser _user, string emailCode)
        {
            StringBuilder emailMessage = new StringBuilder();
            emailMessage.AppendLine("<html>");
            emailMessage.AppendLine("<body>");
            emailMessage.AppendLine($"<p>Dear {_user.Email},</p>");
            emailMessage.AppendLine("<p>You have requested a password reset. To reset your password, please use the following verification code:</p>");
            emailMessage.AppendLine($"<h2>Verification Code: {emailCode}</h2>");
            emailMessage.AppendLine("<p>Please enter this code on our website to reset your password.</p>");
            emailMessage.AppendLine("<p>If you did not request this, please ignore this email.</p?>");
            emailMessage.AppendLine("<br>");
            emailMessage.AppendLine("<p>Best regards,</p?");
            emailMessage.AppendLine("<p><strong>Mike</strong></p<");
            emailMessage.AppendLine("</body>");
            emailMessage.AppendLine("</hmtl>");

            string message = emailMessage.ToString();
            string subject = "Password Reset";
            var emailSent = await _emailService.SendEmail(message, subject, _user.Email);
            if (emailSent){
            return "You have requested a password reset. Kindly check your email for a verification code. ";
            }
            else{
                return "There was a connection error sending the password reset email. Please try again later. ";
            }
        }

        private async Task<string> SendEmail(AppUser user, string emailCode)
        {
            StringBuilder emailMessage = new StringBuilder();
            emailMessage.AppendLine("<html>");
            emailMessage.AppendLine("<body>");
            emailMessage.AppendLine($"<p>Dear {user.Email},</p>");
            emailMessage.AppendLine("<p>Thank you for registering with us. To verify your email address, please use the following verification code:</p>");
            emailMessage.AppendLine($"<h2>Verification Code: {emailCode}</h2>");
            emailMessage.AppendLine("<p>Please enter this code on our website to complete your regsitration.</p>");
            emailMessage.AppendLine("<p>If you did not request this, please ignore this email.</p?>");
            emailMessage.AppendLine("<br>");
            emailMessage.AppendLine("<p>Best regards,</p?");
            emailMessage.AppendLine("<p><strong>Mike</strong></p<");
            emailMessage.AppendLine("</body>");
            emailMessage.AppendLine("</hmtl>");

            
            string message = emailMessage.ToString();
            string subject = "Email Confirmation";
            var emailSent = await _emailService.SendEmail(message, subject, user.Email);
            if (emailSent){
            return "Thank you for registering. Kindly check your email for a confirmation email.";
            }
            else{
                await _userManager.DeleteAsync(user);
                await _context.SaveChangesAsync();

                return "There was a connection error when sending the confirmation email. Please try again later.";
            }
        }

        [HttpPost("confirmation")]
        public async Task<IActionResult> Confirmation([FromBody] ConfirmationDto cfrm)
        {
            if (string.IsNullOrEmpty(cfrm.email) || cfrm.code <= 0)
                return BadRequest("Invalid code provided");
            var user = await GetUser(cfrm.email);
            if (user == null) return BadRequest("Invalide identity provided");

            var result = await _userManager.ConfirmEmailAsync(user, cfrm.code.ToString());
            if (!result.Succeeded)
                return BadRequest("Invalid code provided");
            else
                return Ok("Email confirmed successfully, you can proceed to login");
        }
        private async Task<AppUser> GetUser(string email) => await _userManager.FindByEmailAsync(email);

        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await GetUser(registerDto.Email);
                if (user != null) return BadRequest();


                var appUser = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "Admin");

                    if (roleResult.Succeeded)
                    {
                        var teacher = new Teacher
                        {
                            Name = registerDto.Name,
                            Age = registerDto.Age,
                            Address = registerDto.Address,
                            YearsofExp = registerDto.YearsOfExp,
                            UserId = appUser.Id
                        };

                        _context.Teacher.Add(teacher);
                        await _context.SaveChangesAsync();

                        // return Ok(new newUserDto
                        // {
                        //     UserName = appUser.UserName,
                        //     Email = appUser.Email,
                        //     Token = await _tokenService.CreateToken(appUser)
                        // });
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
                var _user = await GetUser(registerDto.Email);
                var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(_user!);
                var sendEmail = SendEmail(_user!, emailCode);
                return Ok(sendEmail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}