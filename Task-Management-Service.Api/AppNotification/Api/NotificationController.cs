using Task_Management_Service.Domain;
using ServiceDomain = Task_Management_Service.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Task_Management_Service.Api;
[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto createNotificationDto)
    {
        try
        {
            var result = await _notificationService.CreateNotification(createNotificationDto);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpPut("{reference}")]
    public async Task<IActionResult> UpdateNotification(string reference, [FromBody] UpdateNotificationDto updateNotificationDto)
    {
        try
        {
            var result = await _notificationService.UpdateNotification(reference, updateNotificationDto);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpDelete("{reference}")]
    public async Task<IActionResult> DeleteNotification(string reference)
    {
        try
        {
            await GetNotificationByReference(reference);
            var result = await _notificationService.DeleteNotification(reference);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("{reference}")]
    public async Task<IActionResult> GetNotificationByReference(string reference)
    {
        try
        {
            var result = await _notificationService.GetNotificationByReference(reference) ?? throw new NotFoundException($"No notification found with reference: {reference}");
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    // [HttpGet("email/{email}")]
    // public async Task<IActionResult> GetNotificationByEmail(string email)
    // {
    //     try
    //     {
    //         var result = await _notificationService.GetNotificationByEmail(email) ?? throw new NotFoundException($"No notification found with notificationname: {email}");
    //         return Ok(result);
    //     }
    //     catch (AppException e)
    //     {
    //         return StatusCode(e.StatusCode, new AppExceptionResponse(e));
    //     }
    // }

    [HttpGet("list/{page:int:min(1)}")]
    public async Task<IActionResult> GetNotificationList(int page)
    {
        try
        {
            var result = await _notificationService.GetNotificationList(page);
            if (result == null || result.Count == 0)
                throw new NotFoundException($"No notifications found for page: {page}");
            
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpPost("search/{page:int:min(1)}")]
    public async Task<IActionResult> SearchNotificationList(int page,[FromQuery] string type)
    {
        try
        {
            var result = await _notificationService.SearchNotificationList(page, type);
            if (result == null || result.Count == 0)
                throw new NotFoundException($"No notifications found with name: {type} on page: {page}");

            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
}