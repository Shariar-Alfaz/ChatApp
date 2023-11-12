using ChatApp.Application.Feature.Services;
using ChatApp.Infrastructure.Feature.Services.Email.Settings;
using ChatApp.Infrastructure.Feature.Services.Email.Template;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ChatApp.Infrastructure.Feature.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendRegistrationConfirmationEmailAsync(string receiverEmail, string reciverName, string url)
        {
            string subject = "Registration Confirmation";
            var template = new RegistrationConfirmEmail(reciverName, url);
            string message = template.TransformText();
            await SendEmailAsync(receiverEmail, subject, message);
        }

        private async Task SendEmailAsync(string receiverEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailSettings.SenderEmail);
            email.To.Add(MailboxAddress.Parse(receiverEmail));
            email.Subject = subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = message;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSettings.UserName, _emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
