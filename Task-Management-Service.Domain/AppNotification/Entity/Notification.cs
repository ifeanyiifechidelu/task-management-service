

using MongoDB.Bson.Serialization.Attributes;

namespace Task_Management_Service.Domain;
public class Notification
{
    [BsonId]
    public string Reference { get; set; }
    public string Type { get; set; }
    public string Message { get; set; }
    public string UserReference { get; set; }
    public DateTime Timestamp { get; set; }

    public Notification(CreateNotificationDto createNotificationDto)
    {
        Reference = Guid.NewGuid().ToString();
        Type = createNotificationDto.Type;
        Message = createNotificationDto.Message;
        UserReference = createNotificationDto.UserReference;
        Timestamp = DateTime.Now;
    }
    public Notification(UpdateNotificationDto updateNotificationDto)
    {
        Message = updateNotificationDto.Message;
        Type = updateNotificationDto.Type;
        UserReference = updateNotificationDto.UserReference;
        Timestamp = DateTime.Now;
    }
    public Notification()
    {
    }
}