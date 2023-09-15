

namespace Task_Management_Service.Domain;
public interface IUserRepository
{
    Task<string> CreateUser(User user);
    Task<string> UpdateUser(string reference, User user);
    Task<string> DeleteUser(string reference);
    Task<User> GetUserByReference(string reference);
    Task<User> GetUserByEmail(string email);
    Task<List<User>> GetUserList(int page);
    Task<List<User>> SearchUserList(int page, string fullName);
}