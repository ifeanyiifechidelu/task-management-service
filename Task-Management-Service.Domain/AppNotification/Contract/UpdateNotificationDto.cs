using System.ComponentModel.DataAnnotations;

namespace Task_Management_Service.Domain;
public class UpdateNotificationDto
{
    [Required(ErrorMessage = "Type is required.")]
    public string Type { get; set; }
    [Required(ErrorMessage = "Message is required.")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Message should be between 10 and 2000 characters.")]
    public string Message { get; set; }
    [Required(ErrorMessage = "User reference is required.")]
    public string UserReference { get; set; }
}