namespace Inova.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendWelcomeEmailAsync(string to, string userName);
    Task SendConsultantApprovalEmailAsync(string to, string consultantName, bool isApproved);

    Task SendSessionBookedEmailAsync(string consultantEmail, string customerName, DateTime scheduledDate, TimeSpan scheduledTime);
    Task SendSessionAcceptedEmailAsync(string customerEmail, string consultantName, DateTime scheduledDate, TimeSpan scheduledTime);
    Task SendSessionDeniedEmailAsync(string customerEmail, string consultantName);
}