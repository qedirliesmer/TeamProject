using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.Abstracts.Services;
using TeamProject.Application.Options;

namespace TeamProject.Infrastructure.Services;

public class SmtpEmailService : IEmailService
{
    private readonly EmailOptions _options;

    public SmtpEmailService(IOptions<EmailOptions> options)
    {
        _options = options.Value;
    }
    public async Task SendEmailAsync(string to, string subject, string htmlBody, string? plainTextBody = null)
    {
        if (!_options.EnableEmailSending || string.IsNullOrWhiteSpace(to))
        {
            return;
        }
        using var message = new MailMessage();
        message.From = new MailAddress(_options.SenderEmail, _options.SenderName); 
        message.To.Add(to); 
        message.Subject = subject; 

        if (!string.IsNullOrWhiteSpace(htmlBody))
        {
            message.Body = htmlBody;
            message.IsBodyHtml = true; 
        }
        else
        {
            message.Body = plainTextBody;
            message.IsBodyHtml = false; 
        }

        using var client = new SmtpClient(_options.Host, _options.Port)
        {
            EnableSsl = _options.UseSsl,
          Credentials = new NetworkCredential(_options.UserName, _options.Password) 
        };

        await client.SendMailAsync(message);
    }
}
