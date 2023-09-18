

using System.ComponentModel.DataAnnotations;

namespace Task_Management_Service.Domain;
public class UserReferenceDto
{
    [Required(ErrorMessage = "User reference is required.")]
   [StringLength(36, MinimumLength = 36, ErrorMessage = "User reference should be 36 characters (GUID format).")]
    public string UserReference { get; set; }
}