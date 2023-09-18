

namespace Task_Management_Service.Domain;
public interface INotificationRepository
{
    Task<string> CreateNotification(Notification user);
    Task<string> UpdateNotification(string reference, Notification user);
    Task<string> DeleteNotification(string reference);
    Task<Notification> GetNotificationByReference(string reference);
    //Task<Notification> GetNotificationByType(string type);
    Task<List<Notification>> GetNotificationList(int page);
    Task<List<Notification>> SearchNotificationList(int page, string fullName);
}