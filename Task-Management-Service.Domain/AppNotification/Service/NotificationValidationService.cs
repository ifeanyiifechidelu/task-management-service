

namespace Task_Management_Service.Domain;
public class NotificationValidationService:INotificationValidationService
{
    public AppException ValidateCreateNotification(CreateNotificationDto createNotificationDto)
      {
        return new ErrorService().GetValidationExceptionResult(new CreateNotificationValidator().Validate(createNotificationDto));
      }   
      public AppException ValidateUpdateNotification(UpdateNotificationDto updateNotificationDto)
      {
        return new ErrorService().GetValidationExceptionResult(new UpdateNotificationValidator().Validate(updateNotificationDto));
      }   
}
