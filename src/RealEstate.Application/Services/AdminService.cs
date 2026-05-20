using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RealEstate.Application.DTOs.Admin;
using RealEstate.Domain.Common;
using RealEstate.Domain.Entities.User;
using RealEstate.Domain.Enums;
using RealEstate.Domain.Interfaces.Repositories;
using RealEstate.Domain.Interfaces.Services;

namespace RealEstate.Application.Services;

public class AdminService : IAdminService
{
    private readonly IHavliRepository _havliRepo;
    private readonly IDomApartmentRepository _domRepo;
    private readonly IRentalApartmentRepository _rentalRepo;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ILogger<AdminService> _logger;

    public AdminService(
        IHavliRepository havliRepo, IDomApartmentRepository domRepo,
        IRentalApartmentRepository rentalRepo, UserManager<AppUser> userManager,
        IMapper mapper, ILogger<AdminService> logger)
    {
        _havliRepo = havliRepo; _domRepo = domRepo; _rentalRepo = rentalRepo;
        _userManager = userManager; _mapper = mapper; _logger = logger;
    }

    public async Task<GenericResponse<object>> GetDashboardAsync()
    {
        var havlis  = (await _havliRepo.GetAllAsync()).ToList();
        var doms    = (await _domRepo.GetAllAsync()).ToList();
        var rentals = (await _rentalRepo.GetAllAsync()).ToList();
        var users   = _userManager.Users.ToList();

        var sellers = new List<AppUser>();
        foreach (var u in users)
            if (await _userManager.IsInRoleAsync(u, "Seller")) sellers.Add(u);

        var dto = new AdminDashboardDto
        {
            TotalHavli             = havlis.Count,
            TotalDomApartments     = doms.Count,
            TotalRentalApartments  = rentals.Count,
            TotalListings          = havlis.Count + doms.Count + rentals.Count,
            TotalSold              = havlis.Count(x => x.Status == PropertyStatus.Sold) + doms.Count(x => x.Status == PropertyStatus.Sold),
            TotalRented            = rentals.Count(x => x.Status == PropertyStatus.Rented),
            TotalAvailable         = havlis.Count(x => x.Status == PropertyStatus.Available)
                                   + doms.Count(x => x.Status == PropertyStatus.Available)
                                   + rentals.Count(x => x.Status == PropertyStatus.Available),
            TotalInactive          = havlis.Count(x => x.Status == PropertyStatus.Inactive)
                                   + doms.Count(x => x.Status == PropertyStatus.Inactive)
                                   + rentals.Count(x => x.Status == PropertyStatus.Inactive),
            TotalUsers             = users.Count,
            TotalSellers           = sellers.Count,
            TotalBuyers            = users.Count - sellers.Count,
            BlockedUsers           = users.Count(x => x.IsBlocked)
        };
        return GenericResponse<object>.Success(dto);
    }

    public async Task<GenericResponse<object>> GetAllPropertiesAsync()
    {
        var havlis  = await _havliRepo.GetAllAsync();
        var doms    = await _domRepo.GetAllAsync();
        var rentals = await _rentalRepo.GetAllAsync();
        return GenericResponse<object>.Success(new { havlis, doms, rentals });
    }

    public async Task<GenericResponse<object>> ChangePropertyStatusAsync(Guid propertyId, int newStatus, string propertyType)
    {
        var status = (PropertyStatus)newStatus;
        switch (propertyType.ToLower())
        {
            case "havli":
                var h = await _havliRepo.GetByIdAsync(propertyId);
                if (h is null) return GenericResponse<object>.NotFound();
                h.Status = status; h.UpdatedAt = DateTime.UtcNow;
                await _havliRepo.UpdateAsync(h);
                break;
            case "dom":
                var d = await _domRepo.GetByIdAsync(propertyId);
                if (d is null) return GenericResponse<object>.NotFound();
                d.Status = status; d.UpdatedAt = DateTime.UtcNow;
                await _domRepo.UpdateAsync(d);
                break;
            case "rental":
                var r = await _rentalRepo.GetByIdAsync(propertyId);
                if (r is null) return GenericResponse<object>.NotFound();
                r.Status = status; r.UpdatedAt = DateTime.UtcNow;
                await _rentalRepo.UpdateAsync(r);
                break;
            default:
                return GenericResponse<object>.Failure("Unknown property type");
        }
        return GenericResponse<object>.Success(new { }, "Status updated");
    }

    public async Task<GenericResponse<object>> ForceDeletePropertyAsync(Guid propertyId, string propertyType)
    {
        switch (propertyType.ToLower())
        {
            case "havli":  await _havliRepo.DeleteAsync(propertyId);  break;
            case "dom":    await _domRepo.DeleteAsync(propertyId);    break;
            case "rental": await _rentalRepo.DeleteAsync(propertyId); break;
            default: return GenericResponse<object>.Failure("Unknown property type");
        }
        _logger.LogInformation("Admin force-deleted {Type} {Id}", propertyType, propertyId);
        return GenericResponse<object>.Success(new { }, "Property deleted");
    }

    public async Task<GenericResponse<object>> GetAllUsersAsync()
    {
        var users = _userManager.Users.ToList();
        var dtos  = _mapper.Map<List<UserManageDto>>(users);
        foreach (var dto in dtos)
        {
            var user = users.First(u => u.Id == dto.Id);
            dto.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "";
        }
        return GenericResponse<object>.Success(dtos);
    }

    public async Task<GenericResponse<object>> BlockUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return GenericResponse<object>.NotFound();
        user.IsBlocked = !user.IsBlocked;
        await _userManager.UpdateAsync(user);
        var action = user.IsBlocked ? "blocked" : "unblocked";
        return GenericResponse<object>.Success(new { }, $"User {action}");
    }

    public async Task<GenericResponse<object>> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return GenericResponse<object>.NotFound();
        await _userManager.DeleteAsync(user);
        return GenericResponse<object>.Success(new { }, "User deleted");
    }
}
