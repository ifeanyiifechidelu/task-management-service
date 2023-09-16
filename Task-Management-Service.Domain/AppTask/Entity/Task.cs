

using MongoDB.Bson.Serialization.Attributes;

namespace Task_Management_Service.Domain;
public class ServiceTask
{
    [BsonId]
    public string Reference { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Priority { get; set; }
    public string Status { get; set; }
    public string UserReference { get; set; }
    public string ProjectReference { get; set; }

    public ServiceTask(CreateTaskDto createUserDto)
    {
        Reference = Guid.NewGuid().ToString();
        Title = createUserDto.Title;
        Description = createUserDto.Description;
        DueDate = createUserDto.DueDate;
        Priority = createUserDto.Priority;
        Status = createUserDto.Status;
        UserReference = createUserDto.UserReference;
    }
    public ServiceTask(UpdateTaskDto updateUserDto)
    {
        Title = updateUserDto.Title;
        Description = updateUserDto.Description;
        DueDate = updateUserDto.DueDate;
        Priority = updateUserDto.Priority;
        Status = updateUserDto.Status;
        UserReference = updateUserDto.UserReference;
    }
    public ServiceTask()
    {
    }
}