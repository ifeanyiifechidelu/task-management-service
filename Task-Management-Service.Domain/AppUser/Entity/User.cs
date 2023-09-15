

using MongoDB.Bson.Serialization.Attributes;

namespace Task_Management_Service.Domain;
public class User
{
    [BsonId]
    public string Reference { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; private set; }
    public string Email { get; set; }
    public string[] TaskReferences { get; set; }

    public User(CreateUserDto createUserDto)
    {
        Reference = Guid.NewGuid().ToString();
        FirstName = createUserDto.FirstName;
        LastName = createUserDto.LastName;
        FullName = $"{FirstName} {LastName}";
        Email = createUserDto.Email;
        TaskReferences = createUserDto.TaskReferences;
    }
    public User(UpdateUserDto updateUserDto)
    {
        LastName = updateUserDto.LastName;
        FirstName = updateUserDto.FirstName;
        FullName = $"{FirstName} {LastName}";
        Email = updateUserDto.Email;
        TaskReferences = updateUserDto.TaskReferences;
    }
    public User()
    {
    }
}