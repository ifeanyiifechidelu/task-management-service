using System.ComponentModel.DataAnnotations;

namespace Task_Management_Service.Domain;
public class CreateUserDto
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "First name should be between 1 and 100 characters.")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Last name should be between 1 and 100 characters.")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(200, ErrorMessage = "Email should not exceed 200 characters.")]
    public string Email { get; set; }
}