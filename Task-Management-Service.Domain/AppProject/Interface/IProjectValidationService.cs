

namespace Task_Management_Service.Domain;
public interface IProjectValidationService
{
     AppException ValidateCreateProject(CreateProjectDto createProjectDto);
      AppException ValidateUpdateProject(UpdateProjectDto updateProjectDto);
}
