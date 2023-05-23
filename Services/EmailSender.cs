using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Projekt.Options;
using System.Net.Mail;

namespace Projekt.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _senderEmail;

        public EmailSender(IOptions<SmtpOptions> opts)
        {
            _senderEmail = opts.Value.SenderEmail;
            _smtpClient = new SmtpClient(opts.Value.Host, opts.Value.Port);
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await _smtpClient.SendMailAsync(_senderEmail, email, subject, htmlMessage);
        }
    }
}

