

namespace Task_Management_Service.Domain;
public interface ITaskValidationService
{
     AppException ValidateCreateTask(CreateTaskDto createTaskDto);
      AppException ValidateUpdateTask(UpdateTaskDto updateTaskDto);
}
