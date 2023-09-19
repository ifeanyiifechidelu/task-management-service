using Task_Management_Service.Domain;

namespace Task_Management_Service.Api;
public interface ISendNotificationService
{
    Task CheckAndSendNotifications();
}