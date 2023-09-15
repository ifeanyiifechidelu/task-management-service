using Task_Management_Service.Domain;

namespace Task_Management_Service.Api;
public interface IUserService
{
    Task<string> CreateUser(CreateUserDto user);
    Task<string> UpdateUser(string reference, UpdateUserDto user);
    Task<string> DeleteUser(string reference);
    Task<UserDto> GetUserByReference(string reference);
    Task<UserDto> GetUserByEmail(string email);
    Task<List<UserDto>> GetUserList(int page);
    Task<List<UserDto>> SearchUserList(int page, string fullName);
}