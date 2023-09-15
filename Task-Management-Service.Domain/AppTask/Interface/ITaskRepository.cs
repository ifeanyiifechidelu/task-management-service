

namespace Task_Management_Service.Domain;
public interface ITaskRepository
{
    Task<string> CreateTask(ServiceTask task);
    Task<string> UpdateTask(string reference, ServiceTask task);
    Task<string> DeleteTask(string reference);
    Task<ServiceTask> GetTaskByReference(string reference);
    Task<ServiceTask> GetTaskByStatus(string status);
    Task<ServiceTask> GetTaskByPriority(string priority);
    Task<List<ServiceTask>> GetTaskList(int page);
    Task<List<ServiceTask>> SearchTaskList(int page, string title);
}