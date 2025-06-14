using JobTracking.Application.Contracts.Base;
using JobTracking.Domain.DTOs.Request.Create;
using JobTracking.Domain.DTOs.Request.Update;
using JobTracking.Domain.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

namespace JobTracking.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _userService.GetUser(id));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageCount = 10)
    {
        var users = await _userService.GetAllUsers(page, pageCount);
        var response = users.Select(u => new UserResponseDTO
        {
            Id = u.Id,
            FirstName = u.FirstName,
            MiddleName = u.MiddleName,
            LastName = u.LastName,
            Username = u.Username,
            Role = u.Role
        });

        return Ok(response);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetFiltered([FromQuery] string? username)
    {
        var users = await _userService.GetFilteredUsers(username);
        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponseDTO>> Add([FromBody] UserCreateRequestDTO dto)
    {
        var user = await _userService.CreateUser(dto);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserUpdateRequestDTO dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("ID mismatch.");
        }

        var success = await _userService.UpdateUser(dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _userService.DeleteUser(id);
        return success ? NoContent() : NotFound();
    }
}