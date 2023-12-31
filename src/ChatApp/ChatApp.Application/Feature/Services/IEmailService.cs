﻿namespace ChatApp.Application.Feature.Services
{
    public interface IEmailService
    {
        Task SendRegistrationConfirmationEmailAsync(string receiverEmail, string receiverName, string url);
        Task SendForgotPasswordEmailAsync(string receiverEmail, string receiverName, string url);
    }
}
