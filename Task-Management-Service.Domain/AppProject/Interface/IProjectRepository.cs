

namespace Task_Management_Service.Domain;
public interface IProjectRepository
{
    Task<string> CreateProject(Project project);
    Task<string> UpdateProject(string reference, Project project);
    Task<string> DeleteProject(string reference);
    Task<Project> GetProjectByReference(string reference);
    Task<List<Project>> GetProjectList(int page);
    Task<List<Project>> SearchProjectList(int page, string name);
}