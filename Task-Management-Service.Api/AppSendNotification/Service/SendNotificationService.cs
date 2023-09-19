using Task_Management_Service.Domain;
using Serilog;
using System.Text;
using System.Data;

namespace Task_Management_Service.Api;
public class SendNotificationService : ISendNotificationService
{
    private readonly IUserRepository _userRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly INotificationValidationService _notificationValidationService;

    public SendNotificationService(INotificationRepository notificationRepository, INotificationValidationService notificationValidationService, ITaskRepository taskRepository, IUserRepository userRepository)
    {
        _notificationRepository = notificationRepository;
        _notificationValidationService = notificationValidationService;
        _taskRepository = taskRepository;
        _userRepository = userRepository;
    }

    public async Task CheckAndSendNotifications()
    {
        // Get the current time.
        var currentTime = DateTime.Now;

        // Get tasks due within the next 48 hours.
        var tasksDueSoon = await _taskRepository.GetTasksDueWithinHours(48);

        foreach (var task in tasksDueSoon)
        {
            // Find the user associated with this task.
            var user = await _userRepository.GetUserByReference(task.UserReference);

            if (user != null)
            {
                // Send a due date reminder notification to the user.
                var emailContent = $"Task Due Date Reminder: {task.Name} is due on {task.DueDate}.";
                await SendNotificationEmail(user.Email, "Due Date Reminder", emailContent);
            }
        }

        // Check for completed tasks.
        var completedTasks = await _taskRepository.GetCompletedTasks();

        foreach (var task in completedTasks)
        {
            // Find the user associated with this task.
            var user = await _userRepository.GetUserByReference(task.UserReference);

            if (user != null)
            {
                // Send a notification for the completed task.
                var emailContent = $"Task Completed: {task.Name} has been marked as completed.";
                await SendNotificationEmail(user.Email, "Task Completed", emailContent);
            }
        }

        // Check for new assigned tasks (you'll need some mechanism to track new assignments).
        var newAssignedTasks = await _taskRepository.GetNewlyAssignedTasks();

        foreach (var task in newAssignedTasks)
        {
            // Find the user associated with this task.
            var user = await _userRepository.GetUserByReference(task.UserReference);

            if (user != null)
            {
                // Send a notification for the new assigned task.
                var emailContent = $"New Task Assigned: You have been assigned a new task - {task.Name}.";
                await SendNotificationEmail(user.Email, "New Task Assigned", emailContent);
            }
        }
    }

    private async Task SendNotificationEmail(string recipient, string subject, string body)
    {
        // Use your email service to send the notification email.
        var result = await _emailService.SendEmailAsync(recipient, subject, body);

        // You can handle the result (success or failure) as needed.
        if (result.IsSuccess)
        {
            // Log the successful email sending.
            Log.Information($"Email sent successfully to {recipient} with subject: {subject}");
        }
        else
        {
            // Log the email sending failure.
            Log.Error($"Failed to send email to {recipient} with subject: {subject}. Error: {result.Message}");
        }
    }
}
