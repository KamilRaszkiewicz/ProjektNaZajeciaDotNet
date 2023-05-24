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
        private readonly bool _isDevelopment;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(ILogger<EmailSender> logger,IOptions<SmtpOptions> opts, IWebHostEnvironment env)
        {
            _isDevelopment = env.IsDevelopment();
            _senderEmail = opts.Value.SenderEmail;
            _smtpClient = new SmtpClient(opts.Value.Host, opts.Value.Port);
            _logger = logger;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                await _smtpClient.SendMailAsync(_senderEmail, email, subject, htmlMessage);
            }
            catch (SmtpException e) when (_isDevelopment)
            {
                _logger.LogInformation($"Exception thrown: {e.Message}");
            }
        }
    }
}

