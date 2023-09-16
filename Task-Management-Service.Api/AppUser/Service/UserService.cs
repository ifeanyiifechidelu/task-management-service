using Task_Management_Service.Domain;
using Serilog;
using System.Text;
using System.Data;

namespace Task_Management_Service.Api;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IUserValidationService _userValidationService;

    public UserService(IUserRepository userRepository, IUserValidationService userValidationService, ITaskRepository taskRepository)
    {
        _userRepository = userRepository;
        _userValidationService = userValidationService;
        _taskRepository = taskRepository;
    }

    public async Task<string> CreateUser(CreateUserDto userDto)
    {
        try
        {
            var validationException = _userValidationService.ValidateCreateUser(userDto);
            if (validationException != null) throw validationException;

            var availableUser = await _userRepository.GetUserByEmail($"{userDto.Email}");
            if (availableUser != null)
            {
                Log.Warning($"There is already a user found with the given username: {userDto.Email}.");
                throw new ConflictException($"there is already a user found with the given username: {userDto.Email}.");
            }

            var user = new User(userDto);

            return await _userRepository.CreateUser(user);
        }
        catch (BadRequestException e)
        {
            Log.Error($"Bad Request Error: {e.Message}");
            throw;
        }
        catch (ConflictException e)
        {
            Log.Error($"Conflict Error: {e.Message}");
            throw;
        }
        catch (AppException e)
        {
            Log.Error($"Database Error: {e.Message}");
            throw;
        }

        // General error (this should be last in the catch sequence)
        catch (Exception e)
        {
            Log.Error($"Error Creating User: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }

    public async Task<string> UpdateUser(string reference, UpdateUserDto userDto)
    {
        try
        {
            var validationException = _userValidationService.ValidateUpdateUser(userDto);
            if (validationException != null) throw validationException;

            await GetUserByReference(reference);

            var user = new User(userDto)
            {
                Reference = reference
            };

            return await _userRepository.UpdateUser(reference, user);
        }
        catch (BadRequestException e)
        {
            Log.Error($"Bad Request Error: {e.Message}");
            throw;
        }
        catch (AppException e)
        {
            Log.Error($"Database Error: {e.Message}");
            throw;
        }

        // General error (this should be last in the catch sequence)
        catch (Exception e)
        {
            Log.Error($"Error Updating User: {e.Message}");
            throw new InternalServerException(e.Message);
        }

    }

    public async Task<string> DeleteUser(string reference)
    {
        try
        {
            return await _userRepository.DeleteUser(reference);
        }
        catch (BadRequestException e)
        {
            Log.Error($"Bad Request Error: {e.Message}");
            throw;
        }
        catch (AppException e)
        {
            Log.Error($"Database Error: {e.Message}");
            throw;
        }

        // General error (this should be last in the catch sequence)
        catch (Exception e)
        {
            Log.Error($"Error Deleting User: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }

    public async Task<UserDto> GetUserByReference(string reference)
    {
        try
        {
            var user = await _userRepository.GetUserByReference(reference) ?? throw new NotFoundException("User not found by the given reference.");

            var tasks = await _taskRepository.GetTasksByUserReference(reference) ?? throw new NotFoundException("User not found by the given reference.");
            return new UserDto(user.Reference, user.FullName, user.Email, tasks);
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching user by reference: {e.Message}");
        }
    }

    public async Task<UserDto> GetUserByEmail(string email)
    {
        try
        {
            var user = await _userRepository.GetUserByEmail(email) ?? throw new NotFoundException("User not found by the given email.");
            var tasks = await _taskRepository.GetTasksByUserReference(user.Reference) ?? throw new NotFoundException("User not found by the given reference.");
            return new UserDto(user.Reference, user.FullName, user.Email, tasks);
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching user by fullname: {e.Message}");
        }
    }

    public async Task<List<UserDto>> GetUserList(int page)
    {
        try
        {
            var users = await _userRepository.GetUserList(page);

            if (users == null || !users.Any())
                throw new NotFoundException("No users found for the given page.");


            var tasks = new List<List<ServiceTask>>();
            foreach (var user in users)
            {
                var tasksByUser = await _taskRepository.GetTasksByUserReference(user.Reference) ?? throw new NotFoundException("User not found by the given reference.");
                tasks.Add(tasksByUser);
            }

            return users.Zip(tasks, (user, tasksByUser) => new UserDto(user.Reference, user.FullName, user.Email, tasksByUser)).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching user list: {e.Message}");
        }
    }


    public async Task<List<UserDto>> SearchUserList(int page, string fullname)
    {
        try
        {
            var users = await _userRepository.SearchUserList(page, fullname);

            if (users == null || !users.Any())
                throw new NotFoundException("No users found with the given name.");

            var tasks = new List<List<ServiceTask>>();
            foreach (var user in users)
            {
                var tasksByUser = await _taskRepository.GetTasksByUserReference(user.Reference) ?? throw new NotFoundException("User not found by the given reference.");
                tasks.Add(tasksByUser);
            }

            return users.Zip(tasks, (user, tasksByUser) => new UserDto(user.Reference, user.FullName, user.Email, tasksByUser)).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error searching user list: {e.Message}");
        }
    }
}