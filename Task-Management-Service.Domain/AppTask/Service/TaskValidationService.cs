

namespace Task_Management_Service.Domain;
public class TaskValidationService:ITaskValidationService
{
    public AppException ValidateCreateTask(CreateTaskDto createTaskDto)
      {
        return new ErrorService().GetValidationExceptionResult(new CreateTaskValidator().Validate(createTaskDto));
      }   
      public AppException ValidateUpdateTask(UpdateTaskDto updateTaskDto)
      {
        return new ErrorService().GetValidationExceptionResult(new UpdateTaskValidator().Validate(updateTaskDto));
      }
      public AppException ValidateAssignTaskToProject(AssignTaskToProjectDto assignTaskToProjectDto)
      {
        return new ErrorService().GetValidationExceptionResult(new AssignTaskToProjectValidator().Validate(assignTaskToProjectDto));
      }   
}
