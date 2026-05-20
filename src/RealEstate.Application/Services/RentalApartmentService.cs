using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstate.Application.DTOs.Filter;
using RealEstate.Application.DTOs.Property;
using RealEstate.Domain.Common;
using RealEstate.Domain.Entities.Property;
using RealEstate.Domain.Interfaces.Repositories;
using RealEstate.Domain.Interfaces.Services;

namespace RealEstate.Application.Services;

public class RentalApartmentService : IRentalApartmentService
{
    private readonly IRentalApartmentRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<RentalApartmentService> _logger;

    public RentalApartmentService(IRentalApartmentRepository repo, IMapper mapper, ILogger<RentalApartmentService> logger)
    {
        _repo = repo; _mapper = mapper; _logger = logger;
    }

    public async Task<GenericResponse<object>> GetAllAsync(object filterObj)
    {
        var filter = filterObj as RentalFilterDto ?? new RentalFilterDto();
        var all = await _repo.GetAllAsync();
        var q = all.AsQueryable();

        if (filter.MinMonthlyRent.HasValue)  q = q.Where(x => x.MonthlyRent >= filter.MinMonthlyRent);
        if (filter.MaxMonthlyRent.HasValue)  q = q.Where(x => x.MonthlyRent <= filter.MaxMonthlyRent);
        if (filter.MinArea.HasValue)         q = q.Where(x => x.Area >= filter.MinArea);
        if (filter.MaxArea.HasValue)         q = q.Where(x => x.Area <= filter.MaxArea);
        if (filter.MinRooms.HasValue)        q = q.Where(x => x.Rooms >= filter.MinRooms);
        if (filter.MaxRooms.HasValue)        q = q.Where(x => x.Rooms <= filter.MaxRooms);
        if (filter.MinFloor.HasValue)        q = q.Where(x => x.Floor >= filter.MinFloor);
        if (filter.MaxFloor.HasValue)        q = q.Where(x => x.Floor <= filter.MaxFloor);
        if (filter.UtilitiesIncluded.HasValue) q = q.Where(x => x.UtilitiesIncluded == filter.UtilitiesIncluded);
        if (!string.IsNullOrEmpty(filter.Entrance)) q = q.Where(x => x.Entrance == filter.Entrance);
        if (!string.IsNullOrEmpty(filter.Region))   q = q.Where(x => x.Region.Contains(filter.Region));
        if (!string.IsNullOrEmpty(filter.District)) q = q.Where(x => x.District.Contains(filter.District));

        q = filter.SortBy switch
        {
            "price_asc"  => q.OrderBy(x => x.MonthlyRent),
            "price_desc" => q.OrderByDescending(x => x.MonthlyRent),
            "oldest"     => q.OrderBy(x => x.CreatedAt),
            _            => q.OrderByDescending(x => x.CreatedAt)
        };

        var total = q.Count();
        var items = q.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
        var dtos  = _mapper.Map<List<RentalApartmentDto>>(items);
        var paged = new PagedResult<RentalApartmentDto>
        {
            Items = dtos, TotalCount = total,
            PageNumber = filter.PageNumber, PageSize = filter.PageSize
        };
        return GenericResponse<object>.Success(paged);
    }

    public async Task<GenericResponse<object>> GetByIdAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return GenericResponse<object>.NotFound($"Rental {id} not found");
        return GenericResponse<object>.Success(_mapper.Map<RentalApartmentDto>(entity));
    }

    public async Task<GenericResponse<object>> CreateAsync(object dtoObj, string sellerId)
    {
        var dto = dtoObj as RentalApartmentCreateDto;
        if (dto is null) return GenericResponse<object>.Failure("Invalid DTO");
        var entity = _mapper.Map<RentalApartment>(dto);
        entity.SellerId = sellerId;
        await _repo.AddAsync(entity);
        _logger.LogInformation("Rental created: {Id}", entity.Id);
        return GenericResponse<object>.Success(_mapper.Map<RentalApartmentDto>(entity), "Rental created", 201);
    }

    public async Task<GenericResponse<object>> UpdateAsync(Guid id, object dtoObj, string sellerId, bool isAdmin)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return GenericResponse<object>.NotFound();
        if (!isAdmin && entity.SellerId != sellerId) return GenericResponse<object>.Unauthorized();
        _mapper.Map(dtoObj as RentalApartmentUpdateDto, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(entity);
        return GenericResponse<object>.Success(_mapper.Map<RentalApartmentDto>(entity), "Updated");
    }

    public async Task<GenericResponse<object>> DeleteAsync(Guid id, string sellerId, bool isAdmin)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return GenericResponse<object>.NotFound();
        if (!isAdmin && entity.SellerId != sellerId) return GenericResponse<object>.Unauthorized();
        await _repo.DeleteAsync(id);
        return GenericResponse<object>.Success(new { }, "Deleted");
    }

    public async Task<GenericResponse<object>> GetMyListingsAsync(string sellerId)
    {
        var items = await _repo.GetBySellerIdAsync(sellerId);
        return GenericResponse<object>.Success(_mapper.Map<List<RentalApartmentDto>>(items));
    }
}
