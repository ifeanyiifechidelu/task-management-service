using Task_Management_Service.Domain;
using Serilog;
using System.Text;
using System.Data;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Task_Management_Service.Api;
public class EmailService : IEmailService
{
    private readonly IUserRepository _userRepository;
    private readonly INotificationRepository _notificationRepository;

    private readonly ITaskRepository _taskRepository;

    public EmailService(IUserRepository userRepository, INotificationRepository notificationRepository, ITaskRepository taskRepository)
    {
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
        _taskRepository = taskRepository;
    }

    public EmailResult SendEmail(string recipient, string subject, string body)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(Service.MailFrom));
            email.To.Add(MailboxAddress.Parse(recipient));
            email.Subject = subject;
            var builder = new BodyBuilder
            {
                HtmlBody = body
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(Service.MailHost, int.Parse(Service.MailPort), SecureSocketOptions.SslOnConnect);
            smtp.Authenticate(Service.UserName, Service.Password);
            smtp.Send(email);
            smtp.Disconnect(true);

            Log.Information($"Email sent successfully to {recipient} with subject: {subject}");

            return new EmailResult { IsSuccess = true, Message = "Email sent successfully" };
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to send email to {recipient} with subject: {subject}. Error: {ex.Message}");

            return new EmailResult { IsSuccess = false, Message = $"Failed to send email. Error: {ex.Message}" };
        }
    }
}