using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Application.Options;

public class EmailOptions
{
    public const string SectionName = "Email";
 
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool UseSsl { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string SenderEmail { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;

    public string ConfirmationBaseUrl { get; set; } = string.Empty;

    public bool EnableEmailSending { get; set; }
}
