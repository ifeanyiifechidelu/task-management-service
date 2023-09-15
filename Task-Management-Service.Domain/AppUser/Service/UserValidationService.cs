

namespace Task_Management_Service.Domain;
public class UserValidationService:IUserValidationService
{
    public AppException ValidateCreateUser(CreateUserDto createUserDto)
      {
        return new ErrorService().GetValidationExceptionResult(new CreateUserValidator().Validate(createUserDto));
      }   
      public AppException ValidateUpdateUser(UpdateUserDto updateUserDto)
      {
        return new ErrorService().GetValidationExceptionResult(new UpdateUserValidator().Validate(updateUserDto));
      }   
}
