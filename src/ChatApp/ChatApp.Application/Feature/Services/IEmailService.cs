namespace ChatApp.Application.Feature.Services
{
    public interface IEmailService
    {
        Task SendRegistrationConfirmationEmailAsync(string receiverEmail, string reciverName, string url);
    }
}
