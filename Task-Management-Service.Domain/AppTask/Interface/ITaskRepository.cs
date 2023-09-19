

namespace Task_Management_Service.Domain;
public interface ITaskRepository
{
    Task<string> CreateTask(ServiceTask task);
    Task<string> UpdateTask(string reference, ServiceTask task);
    Task<string> DeleteTask(string reference);
    Task<ServiceTask> GetTaskByReference(string reference);
    Task<List<ServiceTask>> GetTasksDueWithinHours(int hours);
    Task<List<ServiceTask>> GetCompletedTasks();
    Task<List<ServiceTask>> GetNewlyAssignedTasks(DateTime cutoffTime);
    Task<List<ServiceTask>> GetTasksByStatus(string status);
    Task<List<ServiceTask>> GetTasksByPriority(string priority);
    Task<List<ServiceTask>> GetTasksDueByCurrentWeek(DateTime weekStartDate, DateTime weekEndDate);
    Task<List<ServiceTask>> GetTasksByUserReference(string userReference);
    Task<List<ServiceTask>> GetTasksByProjectReference(string projectReference);
    Task<List<ServiceTask>> GetTaskList(int page);
    Task<List<ServiceTask>> SearchTaskList(int page, string title);
}