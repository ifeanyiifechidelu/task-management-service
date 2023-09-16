

namespace Task_Management_Service.Domain;
public class UserDto
{
    public string Reference { get; set; }
    public string FullName { get; private set; }
    public string Email { get; set; }
    public List<ServiceTask> Tasks { get; set; }

    public UserDto(string reference, string fullName,string email, List<ServiceTask> tasks)
    {
        Reference = reference;
        FullName = fullName;
        Email = email;
        Tasks = tasks;
    }
}