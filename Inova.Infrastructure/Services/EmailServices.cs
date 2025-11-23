using System.Net;
using System.Net.Mail;
using Inova.Application.Interfaces;
using Inova.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace Inova.Infrastructure.Services;

internal sealed class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfig;

    public EmailService(IOptions<EmailConfiguration> emailConfig)
    {
        _emailConfig = emailConfig.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            using var smtpClient = new SmtpClient(_emailConfig.SmtpServer)
            {
                Port = _emailConfig.SmtpPort,
                Credentials = new NetworkCredential(
                    _emailConfig.SmtpUsername,
                    _emailConfig.SmtpPassword
                ),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailConfig.FromEmail, _emailConfig.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            // Log error (for now just print to console)
            Console.WriteLine($"Email sending failed: {ex.Message}");
            // In production, use proper logging (ILogger)
        }
    }

    public async Task SendWelcomeEmailAsync(string to, string userName)
    {
        var subject = "Welcome to Inova! 🎉";
        var body = $@"
            <html>
            <body>
                <h2>Welcome to Inova, {userName}!</h2>
                <p>Thank you for registering with us.</p>
                <p>You can now access our platform and connect with professional consultants.</p>
                <br/>
                <p>Best regards,</p>
                <p><strong>Inova Team</strong></p>
            </body>
            </html>
        ";

        await SendEmailAsync(to, subject, body);
    }

    public async Task SendConsultantApprovalEmailAsync(string to, string consultantName, bool isApproved)
    {
        var subject = isApproved
            ? "Congratulations! Your Consultant Application is Approved ✅"
            : "Consultant Application Update ❌";

        var body = isApproved
            ? $@"
                <html>
                <body>
                    <h2>Congratulations, {consultantName}!</h2>
                    <p>Your consultant application has been <strong>APPROVED</strong>.</p>
                    <p>You can now start receiving consultation requests from customers.</p>
                    <br/>
                    <p>Best regards,</p>
                    <p><strong>Inova Team</strong></p>
                </body>
                </html>
            "
            : $@"
                <html>
                <body>
                    <h2>Hello, {consultantName}</h2>
                    <p>Unfortunately, your consultant application has been <strong>REJECTED</strong>.</p>
                    <p>If you have questions, please contact our support team.</p>
                    <br/>
                    <p>Best regards,</p>
                    <p><strong>Inova Team</strong></p>
                </body>
                </html>
            ";

        await SendEmailAsync(to, subject, body);
    }
}