using Task_Management_Service.Domain;
using Serilog;
using System.Text;
using System.Data;

namespace Task_Management_Service.Api;
public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectValidationService _projectValidationService;

    public ProjectService(IProjectRepository projectRepository, IProjectValidationService projectValidationService, ITaskRepository taskRepository)
    {
        _projectRepository = projectRepository;
        _projectValidationService = projectValidationService;
        _taskRepository = taskRepository;
    }

    public async Task<string> CreateProject(CreateProjectDto projectDto)
    {
        try
        {
            var validationException = _projectValidationService.ValidateCreateProject(projectDto);
            if (validationException != null) throw validationException;

            var project = new Project(projectDto);

            return await _projectRepository.CreateProject(project);
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
            Log.Error($"Error Creating Project: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }

    public async Task<string> UpdateProject(string reference, UpdateProjectDto projectDto)
    {
        try
        {
            var validationException = _projectValidationService.ValidateUpdateProject(projectDto);
            if (validationException != null) throw validationException;

            await GetProjectByReference(reference);

            var project = new Project(projectDto)
            {
                Reference = reference
            };

            return await _projectRepository.UpdateProject(reference, project);
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
            Log.Error($"Error Updating Project: {e.Message}");
            throw new InternalServerException(e.Message);
        }

    }

    public async Task<string> DeleteProject(string reference)
    {
        try
        {
            return await _projectRepository.DeleteProject(reference);
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
            Log.Error($"Error Deleting Project: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }

    public async Task<ProjectDto> GetProjectByReference(string reference)
    {
        try
        {
            var project = await _projectRepository.GetProjectByReference(reference) ?? throw new NotFoundException("Project not found by the given reference.");

            var tasks = await _taskRepository.GetTasksByProjectReference(reference) ?? throw new NotFoundException("Project not found by the given reference.");
            return new ProjectDto(project.Reference, project.Name, project.Description, tasks);
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching project by reference: {e.Message}");
        }
    }

    public async Task<List<ProjectDto>> GetProjectList(int page)
    {
        try
        {
            var projects = await _projectRepository.GetProjectList(page);

            if (projects == null || !projects.Any())
                throw new NotFoundException("No projects found for the given page.");


            var tasks = new List<List<ServiceTask>>();
            foreach (var project in projects)
            {
                var tasksByProject = await _taskRepository.GetTasksByProjectReference(project.Reference) ?? throw new NotFoundException("Project not found by the given reference.");
                tasks.Add(tasksByProject);
            }

            return projects.Zip(tasks, (project, tasksByProject) => new ProjectDto(project.Reference, project.Name, project.Description, tasksByProject)).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching project list: {e.Message}");
        }
    }


    public async Task<List<ProjectDto>> SearchProjectList(int page, string name)
    {
        try
        {
            var projects = await _projectRepository.SearchProjectList(page, name);

            if (projects == null || !projects.Any())
                throw new NotFoundException("No projects found with the given name.");

            var tasks = new List<List<ServiceTask>>();
            foreach (var project in projects)
            {
                var tasksByProject = await _taskRepository.GetTasksByProjectReference(project.Reference) ?? throw new NotFoundException("Project not found by the given reference.");
                tasks.Add(tasksByProject);
            }

            return projects.Zip(tasks, (project, tasksByProject) => new ProjectDto(project.Reference, project.Name, project.Description, tasksByProject)).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error searching project list: {e.Message}");
        }
    }
}