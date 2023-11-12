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
            const string subject = "Registration Confirmation";
            var template = new RegistrationConfirmEmail(reciverName, url);
            var message = template.TransformText();
            await SendEmailAsync(receiverEmail, subject, message);
        }

        public async Task SendForgotPasswordEmailAsync(string receiverEmail, string reciverName, string url)
        {
            const string subject = "Forgot Password";
            var template = new ForgotPasswordEmail(reciverName, url);
            var message = template.TransformText();
            await SendEmailAsync(receiverEmail, subject, message);
        }

        private async Task SendEmailAsync(string receiverEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailSettings.SenderEmail);
            email.To.Add(MailboxAddress.Parse(receiverEmail));
            email.Subject = subject;
            var builder = new BodyBuilder
            {
                HtmlBody = message
            };
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            
            await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
