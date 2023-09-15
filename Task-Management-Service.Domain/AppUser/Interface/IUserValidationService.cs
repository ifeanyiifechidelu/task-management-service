

namespace Task_Management_Service.Domain;
public interface IUserValidationService
{
     AppException ValidateCreateUser(CreateUserDto createUserDto);
      AppException ValidateUpdateUser(UpdateUserDto updateUserDto);
}
