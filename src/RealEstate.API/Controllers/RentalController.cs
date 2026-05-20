using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs.Filter;
using RealEstate.Application.DTOs.Property;
using RealEstate.Domain.Interfaces.Services;
using System.Security.Claims;

namespace RealEstate.API.Controllers;

[ApiController]
[Route("api/rental")]
public class RentalController : ControllerBase
{
    private readonly IRentalApartmentService _service;
    public RentalController(IRentalApartmentService service) => _service = service;

    private string UserId  => User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
    private bool   IsAdmin => User.IsInRole("Admin");

    /// <summary>Get all Rental apartments with filter and pagination</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] RentalFilterDto filter)
    {
        var result = await _service.GetAllAsync(filter);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Get a single Rental by ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Create a Rental listing [Seller/Admin]</summary>
    [Authorize(Roles = "Seller,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RentalApartmentCreateDto dto)
    {
        var result = await _service.CreateAsync(dto, UserId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Update a Rental listing [Owner/Admin]</summary>
    [Authorize(Roles = "Seller,Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] RentalApartmentUpdateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto, UserId, IsAdmin);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Delete a Rental listing [Owner/Admin]</summary>
    [Authorize(Roles = "Seller,Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id, UserId, IsAdmin);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Seller's own Rental listings</summary>
    [Authorize(Roles = "Seller,Admin")]
    [HttpGet("my-listings")]
    public async Task<IActionResult> MyListings()
    {
        var result = await _service.GetMyListingsAsync(UserId);
        return StatusCode(result.StatusCode, result);
    }
}
