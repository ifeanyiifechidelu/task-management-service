

using System.ComponentModel.DataAnnotations;

namespace Task_Management_Service.Domain;
public class NotificationToUserDto
{
    [Required(ErrorMessage = "User reference is required.")]
   [StringLength(36, MinimumLength = 36, ErrorMessage = "User reference should be 36 characters (GUID format).")]
    public string UserReference { get; set; }
    [Required(ErrorMessage = "Notification reference is required.")]
   [StringLength(36, MinimumLength = 36, ErrorMessage = "Notification reference should be 36 characters (GUID format).")]
    public string NotificationReference { get; set; }
    [Required(ErrorMessage = "Task reference is required.")]
   [StringLength(36, MinimumLength = 36, ErrorMessage = "Task reference should be 36 characters (GUID format).")]
    public string TaskReference { get; set; }
    public string NotificationContent { get; set; }

}