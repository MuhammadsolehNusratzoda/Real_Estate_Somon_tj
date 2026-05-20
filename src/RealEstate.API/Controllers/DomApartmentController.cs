using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs.Filter;
using RealEstate.Application.DTOs.Property;
using RealEstate.Domain.Interfaces.Services;
using System.Security.Claims;

namespace RealEstate.API.Controllers;

[ApiController]
[Route("api/dom")]
public class DomApartmentController : ControllerBase
{
    private readonly IDomApartmentService _service;
    public DomApartmentController(IDomApartmentService service) => _service = service;

    private string UserId  => User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
    private bool   IsAdmin => User.IsInRole("Admin");

    /// <summary>Get all Dom apartments with filter and pagination</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] DomFilterDto filter)
    {
        var result = await _service.GetAllAsync(filter);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Get a single Dom apartment by ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Create a Dom apartment listing [Seller/Admin]</summary>
    [Authorize(Roles = "Seller,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DomApartmentCreateDto dto)
    {
        var result = await _service.CreateAsync(dto, UserId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Update a Dom apartment [Owner/Admin]</summary>
    [Authorize(Roles = "Seller,Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] DomApartmentUpdateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto, UserId, IsAdmin);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Delete a Dom apartment [Owner/Admin]</summary>
    [Authorize(Roles = "Seller,Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id, UserId, IsAdmin);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Seller's own Dom listings</summary>
    [Authorize(Roles = "Seller,Admin")]
    [HttpGet("my-listings")]
    public async Task<IActionResult> MyListings()
    {
        var result = await _service.GetMyListingsAsync(UserId);
        return StatusCode(result.StatusCode, result);
    }
}
