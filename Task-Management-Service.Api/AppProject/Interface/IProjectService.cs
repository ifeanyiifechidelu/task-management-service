using Task_Management_Service.Domain;

namespace Task_Management_Service.Api;
public interface IProjectService
{
    Task<string> CreateProject(CreateProjectDto project);
    Task<string> UpdateProject(string reference, UpdateProjectDto project);
    Task<string> DeleteProject(string reference);
    Task<ProjectDto> GetProjectByReference(string reference);
    Task<List<ProjectDto>> GetProjectList(int page);
    Task<List<ProjectDto>> SearchProjectList(int page, string fullName);
}