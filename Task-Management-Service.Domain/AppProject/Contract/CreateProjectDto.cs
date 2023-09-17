using System.ComponentModel.DataAnnotations;

namespace Task_Management_Service.Domain;
public class CreateProjectDto
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Name should be between 1 and 100 characters.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description should be between 10 and 2000 characters.")]
    public string Description { get; set; }
}