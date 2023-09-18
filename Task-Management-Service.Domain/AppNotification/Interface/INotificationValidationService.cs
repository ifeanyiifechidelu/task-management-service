

namespace Task_Management_Service.Domain;
public interface INotificationValidationService
{
     AppException ValidateCreateNotification(CreateNotificationDto createNotificationDto);
      AppException ValidateUpdateNotification(UpdateNotificationDto updateNotificationDto);
}
