using Task_Management_Service.Domain;
using ServiceDomain = Task_Management_Service.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Task_Management_Service.Api;
[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTaskDto)
    {
        try
        {
            var result = await _taskService.CreateTask(createTaskDto);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpPut("update-task/{reference}")]
    public async Task<IActionResult> UpdateTask(string reference, [FromBody] UpdateTaskDto updateTaskDto)
    {
        try
        {
            var result = await _taskService.UpdateTask(reference, updateTaskDto);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpPut("assign-task-to-project/{taskReference}")]
    public async Task<IActionResult> AssignTaskToProject(string taskReference, [FromBody] AssignTaskToProjectDto assignTaskToProjectDto)
    {
        try
        {
            var result = await _taskService.AssignTaskToProject(taskReference, assignTaskToProjectDto);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpDelete("{reference}")]
    public async Task<IActionResult> DeleteTask(string reference)
    {
        try
        {
            await GetTaskByReference(reference);
            var result = await _taskService.DeleteTask(reference);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("{reference}")]
    public async Task<IActionResult> GetTaskByReference(string reference)
    {
        try
        {
            var result = await _taskService.GetTaskByReference(reference) ?? throw new NotFoundException($"No task found with reference: {reference}");
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetTasksByStatus(string status)
    {
        try
        {
            var result = await _taskService.GetTasksByStatus(status);
            if (result == null || result.Count == 0)
                throw new NotFoundException($"No tasks found for status: {status}");

            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("priority/{priority}")]
    public async Task<IActionResult> GetTasksByPriority(string priority)
    {
        try
        {
            var result = await _taskService.GetTasksByPriority(priority);
            if (result == null || result.Count == 0)
                throw new NotFoundException($"No tasks found for priority: {priority}");

            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("due-by current-week")]
    public async Task<IActionResult> GetTasksDueByCurrentWeek()
    {
        try
        {
            var result = await _taskService.GetTasksDueByCurrentWeek();
            if (result == null || result.Count == 0)
                throw new NotFoundException($"No tasks due for this current week found");

            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("list/{page:int:min(1)}")]
    public async Task<IActionResult> GetTaskList(int page)
    {
        try
        {
            var result = await _taskService.GetTaskList(page);
            if (result == null || result.Count == 0)
                throw new NotFoundException($"No tasks found for page: {page}");
            
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpPost("search/{title}/{page:int:min(1)}")]
    public async Task<IActionResult> SearchTaskList(int page,string title)
    {
        try
        {
            var result = await _taskService.SearchTaskList(page, title);
            if (result == null || result.Count == 0)
                throw new NotFoundException($"No tasks found with title: {title} on page: {page}");
            
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
}