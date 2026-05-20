using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Domain.Interfaces.Services;

namespace RealEstate.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _service;
    public AdminController(IAdminService service) => _service = service;

    /// <summary>Get dashboard statistics [Admin]</summary>
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var result = await _service.GetDashboardAsync();
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Get all properties across all types [Admin]</summary>
    [HttpGet("properties")]
    public async Task<IActionResult> GetAllProperties()
    {
        var result = await _service.GetAllPropertiesAsync();
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Change property status [Admin]</summary>
    [HttpPut("properties/{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromQuery] int status, [FromQuery] string type)
    {
        var result = await _service.ChangePropertyStatusAsync(id, status, type);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Force-delete any property [Admin]</summary>
    [HttpDelete("properties/{id:guid}")]
    public async Task<IActionResult> ForceDelete(Guid id, [FromQuery] string type)
    {
        var result = await _service.ForceDeletePropertyAsync(id, type);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Get all users [Admin]</summary>
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _service.GetAllUsersAsync();
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Block or unblock a user [Admin]</summary>
    [HttpPut("users/{userId}/block")]
    public async Task<IActionResult> BlockUser(string userId)
    {
        var result = await _service.BlockUserAsync(userId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Delete a user [Admin]</summary>
    [HttpDelete("users/{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var result = await _service.DeleteUserAsync(userId);
        return StatusCode(result.StatusCode, result);
    }
}
