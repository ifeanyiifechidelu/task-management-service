using Task_Management_Service.Domain;
using Serilog;
using System.Text;
using System.Data;

namespace Task_Management_Service.Api;
public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly INotificationValidationService _notificationValidationService;

    public NotificationService(INotificationRepository notificationRepository, INotificationValidationService notificationValidationService, ITaskRepository taskRepository)
    {
        _notificationRepository = notificationRepository;
        _notificationValidationService = notificationValidationService;
        _taskRepository = taskRepository;
    }

    public async Task<string> CreateNotification(CreateNotificationDto notificationDto)
    {
        try
        {
            var validationException = _notificationValidationService.ValidateCreateNotification(notificationDto);
            if (validationException != null) throw validationException;

            var notification = new Notification(notificationDto);

            return await _notificationRepository.CreateNotification(notification);
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
            Log.Error($"Error Creating Notification: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }

    public async Task<string> UpdateNotification(string reference, UpdateNotificationDto notificationDto)
    {
        try
        {
            var validationException = _notificationValidationService.ValidateUpdateNotification(notificationDto);
            if (validationException != null) throw validationException;

            await GetNotificationByReference(reference);

            var notification = new Notification(notificationDto)
            {
                Reference = reference
            };

            return await _notificationRepository.UpdateNotification(reference, notification);
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
            Log.Error($"Error Updating Notification: {e.Message}");
            throw new InternalServerException(e.Message);
        }

    }

    public async Task<string> DeleteNotification(string reference)
    {
        try
        {
            return await _notificationRepository.DeleteNotification(reference);
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
            Log.Error($"Error Deleting Notification: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }

    public async Task<NotificationDto> GetNotificationByReference(string reference)
    {
        try
        {
            var notification = await _notificationRepository.GetNotificationByReference(reference) ?? throw new NotFoundException("Notification not found by the given reference.");

            return new NotificationDto(notification.Reference, notification.Type, notification.Message, notification.UserReference);
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching notification by reference: {e.Message}");
        }
    }

    // public async Task<NotificationDto> GetNotificationByEmail(string email)
    // {
    //     try
    //     {
    //         var notification = await _notificationRepository.GetNotificationByEmail(email) ?? throw new NotFoundException("Notification not found by the given email.");
    //         return new NotificationDto(notification.Reference, notification.Type, notification.Message, notification.UserReference);
    //     }
    //     catch (AppException)  // Catching known exceptions
    //     {
    //         throw;
    //     }
    //     catch (Exception e)  // Catching unexpected exceptions
    //     {
    //         throw new InternalServerException($"Error fetching notification by fullname: {e.Message}");
    //     }
    // }

    public async Task<List<NotificationDto>> GetNotificationList(int page)
    {
        try
        {
            var notifications = await _notificationRepository.GetNotificationList(page);

            if (notifications == null || !notifications.Any())
                throw new NotFoundException("No notifications found for the given page.");


            return notifications.Select(notification => new NotificationDto(notification.Reference, notification.Type, notification.Message, notification.UserReference)).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching notification list: {e.Message}");
        }
    }


    public async Task<List<NotificationDto>> SearchNotificationList(int page, string fullname)
    {
        try
        {
            var notifications = await _notificationRepository.SearchNotificationList(page, fullname);

            if (notifications == null || !notifications.Any())
                throw new NotFoundException("No notifications found with the given name.");

            return notifications.Select(notification => new NotificationDto(notification.Reference, notification.Type, notification.Message, notification.UserReference)).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error searching notification list: {e.Message}");
        }
    }
}