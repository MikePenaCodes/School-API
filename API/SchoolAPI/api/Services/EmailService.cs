using System;
using api.DTOs;
using MimeKit;
using MailKit.Net.Smtp;
using DotNetEnv;

namespace api.Services;

public class EmailService : IEmailService
{
    public async Task<bool> SendEmail(string body, string subject, string to)
    {
        Env.Load();

        var smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER");
        var from = Environment.GetEnvironmentVariable("SMTP_USERNAME");
        var password = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
        var port = 587;

        var _email = new MimeMessage();
        _email.From.Add(MailboxAddress.Parse(from));
        _email.To.Add(MailboxAddress.Parse(to));
        _email.Subject = subject;
        _email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();
        smtp.Connect(smtpServer, port, MailKit.Security.SecureSocketOptions.StartTls);
        smtp.Authenticate(from, password);
        smtp.Send(_email);
        smtp.Disconnect(true);
        return true;
    }
}
