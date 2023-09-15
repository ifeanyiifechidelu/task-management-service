

namespace Task_Management_Service.Domain;
public class TaskDto
{
    public string Reference { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Priority { get; set; }
    public string Status { get; set; }
}