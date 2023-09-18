using System.ComponentModel.DataAnnotations;

namespace Task_Management_Service.Domain;
public class CreateTaskDto
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Title should be between 5 and 200 characters.")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description should be between 10 and 2000 characters.")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Date Time is required.")]
    [DataType(DataType.DateTime)]
    public DateTime DueDate { get; set; }
    [Required(ErrorMessage = "Priority is required.")]
    public string Priority { get; set; }
    [Required(ErrorMessage = "Status is required.")]
    public string Status { get; set; }
    [Required(ErrorMessage = "User reference is required.")]
    public string UserReference { get; set; }
    [Required(ErrorMessage = "Project reference is required.")]
    public string ProjectReference { get; set; }

}