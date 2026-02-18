using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Application.Abstracts.Services;

public interface IEmailService
{
    Task SendEmailAsync(
        string to,
        string subject,
        string htmlBody,
        string? plainTextBody = null);
}
