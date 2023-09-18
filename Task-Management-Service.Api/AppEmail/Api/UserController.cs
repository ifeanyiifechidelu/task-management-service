using Task_Management_Service.Domain;
using ServiceDomain = Task_Management_Service.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Task_Management_Service.Api;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            var result = await _userService.CreateUser(createUserDto);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpPut("{reference}")]
    public async Task<IActionResult> UpdateUser(string reference, [FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            var result = await _userService.UpdateUser(reference, updateUserDto);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpDelete("{reference}")]
    public async Task<IActionResult> DeleteUser(string reference)
    {
        try
        {
            await GetUserByReference(reference);
            var result = await _userService.DeleteUser(reference);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("{reference}")]
    public async Task<IActionResult> GetUserByReference(string reference)
    {
        try
        {
            var result = await _userService.GetUserByReference(reference) ?? throw new NotFoundException($"No user found with reference: {reference}");
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        try
        {
            var result = await _userService.GetUserByEmail(email) ?? throw new NotFoundException($"No user found with username: {email}");
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("list/{page:int:min(1)}")]
    public async Task<IActionResult> GetUserList(int page)
    {
        try
        {
            var result = await _userService.GetUserList(page);
            if (result == null || result.Count == 0)
                throw new NotFoundException($"No users found for page: {page}");
            
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpPost("search/{page:int:min(1)}")]
    public async Task<IActionResult> SearchUserList(int page,[FromQuery] string fullname)
    {
        try
        {
            var result = await _userService.SearchUserList(page, fullname);
            if (result == null || result.Count == 0)
                throw new NotFoundException($"No users found with name: {fullname} on page: {page}");
            
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
}