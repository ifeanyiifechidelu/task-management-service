using Task_Management_Service.Domain;
using Serilog;
using System.Text;
using System.Data;

namespace Task_Management_Service.Api;
public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ITaskValidationService _taskValidationService;

    public TaskService(ITaskRepository taskRepository, ITaskValidationService taskValidationService, IProjectRepository projectRepository)
    {
        _taskRepository = taskRepository;
        _taskValidationService = taskValidationService;
        _projectRepository = projectRepository;
    }

    public async Task<string> CreateTask(CreateTaskDto taskDto)
    {
        try
        {
            var validationException = _taskValidationService.ValidateCreateTask(taskDto);
            if (validationException != null) throw validationException;

            var task = new ServiceTask(taskDto);

            return await _taskRepository.CreateTask(task);
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
            Log.Error($"Error Creating Task: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }

    public async Task<string> UpdateTask(string reference, UpdateTaskDto taskDto)
    {
        try
        {
            var validationException = _taskValidationService.ValidateUpdateTask(taskDto);
            if (validationException != null) throw validationException;

            await GetTaskByReference(reference);

            var task = new ServiceTask(taskDto)
            {
                Reference = reference
            };

            return await _taskRepository.UpdateTask(reference, task);
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
            Log.Error($"Error Updating Task: {e.Message}");
            throw new InternalServerException(e.Message);
        }

    }

    public async Task<string> AssignTaskToProject(string taskReference, AssignTaskToProjectDto assignTaskToProjectDto)
    {
        try
        {
            var validationException = _taskValidationService.ValidateAssignTaskToProject(assignTaskToProjectDto);
            if (validationException != null) throw validationException;

            var task = await _taskRepository.GetTaskByReference(taskReference) ?? throw new NotFoundException("No tasks found with the given user reference.");
            var project = await _projectRepository.GetProjectByReference(assignTaskToProjectDto.ProjectReference) ?? throw new NotFoundException("No tasks found with the given project reference.");

            // Assign the task to the project by setting the task's ProjectReference property
            task.ProjectReference = assignTaskToProjectDto.ProjectReference;

            return await _taskRepository.UpdateTask(taskReference, task);
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching task by user reference: {e.Message}");
        }
    }

    public async Task<string> DeleteTask(string reference)
    {
        try
        {
            return await _taskRepository.DeleteTask(reference);
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
            Log.Error($"Error Deleting Task: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }

    public async Task<TaskDto> GetTaskByReference(string reference)
    {
        try
        {
            var task = await _taskRepository.GetTaskByReference(reference) ?? throw new NotFoundException("Task not found by the given reference.");
            return new TaskDto
            {
                Reference = task.Reference,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status
            };
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching task by reference: {e.Message}");
        }
    }

    public async Task<List<TaskDto>> GetTasksByStatus(string status)
    {
        try
        {
            var tasks = await _taskRepository.GetTasksByStatus(status);
            if (tasks == null || !tasks.Any())
                throw new NotFoundException("No tasks found with the given status.");
                
            return tasks.Select(task => new TaskDto
            {
                Reference = task.Reference,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status
            }).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching task by status: {e.Message}");
        }
    }

    public async Task<List<TaskDto>> GetTasksByPriority(string priority)
    {
        try
        {
            var tasks = await _taskRepository.GetTasksByPriority(priority);
            if (tasks == null || !tasks.Any())
                throw new NotFoundException("No tasks found with the given priority.");

            return tasks.Select(task => new TaskDto
            {
                Reference = task.Reference,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status
            }).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching task by priority: {e.Message}");
        }
    }

    public async Task<List<TaskDto>> GetTasksDueByCurrentWeek()
    {
        try
        {

            // Calculate the start and end date for the current week
        DateTime currentDate = DateTime.Now.Date;
        DateTime weekStartDate = currentDate.AddDays(-(int)currentDate.DayOfWeek); // Start of the week (Sunday)
        DateTime weekEndDate = weekStartDate.AddDays(6); // End of the week (Saturday)

            var tasks = await _taskRepository.GetTasksDueByCurrentWeek(weekStartDate,weekEndDate);
            if (tasks == null || !tasks.Any())
                throw new NotFoundException("No tasks due for this current week found.");

            return tasks.Select(task => new TaskDto
            {
                Reference = task.Reference,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status
            }).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching task due for the current week: {e.Message}");
        }
    }

    public async Task<List<TaskDto>> GetTaskList(int page)
    {
        try
        {
            var tasks = await _taskRepository.GetTaskList(page);

            if (tasks == null || !tasks.Any())
                throw new NotFoundException("No tasks found for the given page.");

            return tasks.Select(task => new TaskDto
            {
                Reference = task.Reference,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status
            }).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching task list: {e.Message}");
        }
    }


    public async Task<List<TaskDto>> SearchTaskList(int page, string title)
    {
        try
        {
            var tasks = await _taskRepository.SearchTaskList(page, title);

            if (tasks == null || !tasks.Any())
                throw new NotFoundException("No tasks found with the given title.");

            return tasks.Select(task => new TaskDto
            {
                Reference = task.Reference,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status
            }).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error searching task list: {e.Message}");
        }
    }
}