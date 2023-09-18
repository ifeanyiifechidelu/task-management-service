

namespace Task_Management_Service.Domain;
public class NotificationDto
{
    public string Reference { get; set; }
    public string Type { get; set; }
    public string Message { get; set; }
    public string UserReference { get; set; }

    public NotificationDto(string reference, string type,string message, string userReference)
    {
        Reference = reference;
        Type = type;
        Message = message;
        UserReference = userReference;
    }
}