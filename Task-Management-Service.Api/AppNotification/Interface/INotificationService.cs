using Task_Management_Service.Domain;

namespace Task_Management_Service.Api;
public interface INotificationService
{
    Task<string> CreateNotification(CreateNotificationDto notification);
    Task<string> UpdateNotification(string reference, UpdateNotificationDto notification);
    Task<string> DeleteNotification(string reference);
    Task<NotificationDto> GetNotificationByReference(string reference);
    //Task<NotificationDto> GetNotificationByEmail(string email);
    Task<List<NotificationDto>> GetNotificationList(int page);
    Task<List<NotificationDto>> SearchNotificationList(int page, string fullName);
}