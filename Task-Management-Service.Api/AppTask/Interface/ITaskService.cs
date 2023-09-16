using Task_Management_Service.Domain;

namespace Task_Management_Service.Api;
public interface ITaskService
{
    Task<string> CreateTask(CreateTaskDto task);
    Task<string> UpdateTask(string reference, UpdateTaskDto task);
    Task<string> DeleteTask(string reference);
    Task<TaskDto> GetTaskByReference(string reference);
    Task<List<TaskDto>> GetTasksByStatus(string status);
    Task<List<TaskDto>> GetTasksByPriority(string priority);
    Task<List<TaskDto>> GetTasksDueByCurrentWeek();
    Task<List<TaskDto>> GetTasksByUserReference(string userReference);
    Task<List<TaskDto>> GetTaskList(int page);
    Task<List<TaskDto>> SearchTaskList(int page, string fullName);
}