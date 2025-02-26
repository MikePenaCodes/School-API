using System;
using api.DTOs;

namespace api.Services;

public interface IEmailService
{
    Task<bool> SendEmail(string body, string subject, string to);


}
