using Task_Management_Service.Domain;
using ServiceDomain = Task_Management_Service.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Task_Management_Service.Api;
[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto createProjectDto)
    {
        try
        {
            var result = await _projectService.CreateProject(createProjectDto);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpPut("{reference}")]
    public async Task<IActionResult> UpdateProject(string reference, [FromBody] UpdateProjectDto updateProjectDto)
    {
        try
        {
            var result = await _projectService.UpdateProject(reference, updateProjectDto);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpDelete("{reference}")]
    public async Task<IActionResult> DeleteProject(string reference)
    {
        try
        {
            await GetProjectByReference(reference);
            var result = await _projectService.DeleteProject(reference);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("{reference}")]
    public async Task<IActionResult> GetProjectByReference(string reference)
    {
        try
        {
            var result = await _projectService.GetProjectByReference(reference) ?? throw new NotFoundException($"No project found with reference: {reference}");
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("list/{page:int:min(1)}")]
    public async Task<IActionResult> GetProjectList(int page)
    {
        try
        {
            var result = await _projectService.GetProjectList(page);
            if (result == null || result.Count == 0)
                throw new NotFoundException($"No projects found for page: {page}");

            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpPost("search/{page:int:min(1)}")]
    public async Task<IActionResult> SearchProjectList(int page,[FromQuery] string fullname)
    {
        try
        {
            var result = await _projectService.SearchProjectList(page, fullname);
            if (result == null || result.Count == 0)
                throw new NotFoundException($"No projects found with name: {fullname} on page: {page}");
            
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
}