

namespace Task_Management_Service.Domain;
public class ProjectValidationService:IProjectValidationService
{
    public AppException ValidateCreateProject(CreateProjectDto createProjectDto)
      {
        return new ErrorService().GetValidationExceptionResult(new CreateProjectValidator().Validate(createProjectDto));
      }   
      public AppException ValidateUpdateProject(UpdateProjectDto updateProjectDto)
      {
        return new ErrorService().GetValidationExceptionResult(new UpdateProjectValidator().Validate(updateProjectDto));
      }   
}
