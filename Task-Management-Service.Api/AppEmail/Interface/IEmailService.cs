using Task_Management_Service.Domain;

namespace Task_Management_Service.Api;
public interface IEmailService
{
    EmailResult SendEmail(string recipient, string subject, string body);
}