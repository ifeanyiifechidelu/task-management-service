

namespace Task_Management_Service.Domain;
public class ProjectDto
{
    public string Reference { get; set; }
    public string Name { get; private set; }
    public string Description { get; set; }
    public List<ServiceTask> Tasks { get; set; }

    public ProjectDto(string reference, string name,string description, List<ServiceTask> tasks)
    {
        Reference = reference;
        Name = name;
        Description = description;
        Tasks = tasks;
    }
}