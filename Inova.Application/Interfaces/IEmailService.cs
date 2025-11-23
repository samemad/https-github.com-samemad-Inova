namespace Inova.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendWelcomeEmailAsync(string to, string userName);
    Task SendConsultantApprovalEmailAsync(string to, string consultantName, bool isApproved);
}