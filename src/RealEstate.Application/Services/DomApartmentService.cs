using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstate.Application.DTOs.Filter;
using RealEstate.Application.DTOs.Property;
using RealEstate.Domain.Common;
using RealEstate.Domain.Entities.Property;
using RealEstate.Domain.Interfaces.Repositories;
using RealEstate.Domain.Interfaces.Services;

namespace RealEstate.Application.Services;

public class DomApartmentService : IDomApartmentService
{
    private readonly IDomApartmentRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<DomApartmentService> _logger;

    public DomApartmentService(IDomApartmentRepository repo, IMapper mapper, ILogger<DomApartmentService> logger)
    {
        _repo = repo; _mapper = mapper; _logger = logger;
    }

    public async Task<GenericResponse<object>> GetAllAsync(object filterObj)
    {
        var filter = filterObj as DomFilterDto ?? new DomFilterDto();
        var all = await _repo.GetAllAsync();
        var q = all.AsQueryable();

        if (filter.MinPrice.HasValue)    q = q.Where(x => x.Price >= filter.MinPrice);
        if (filter.MaxPrice.HasValue)    q = q.Where(x => x.Price <= filter.MaxPrice);
        if (filter.MinArea.HasValue)     q = q.Where(x => x.Area >= filter.MinArea);
        if (filter.MaxArea.HasValue)     q = q.Where(x => x.Area <= filter.MaxArea);
        if (filter.MinRooms.HasValue)    q = q.Where(x => x.Rooms >= filter.MinRooms);
        if (filter.MaxRooms.HasValue)    q = q.Where(x => x.Rooms <= filter.MaxRooms);
        if (filter.MinFloor.HasValue)    q = q.Where(x => x.Floor >= filter.MinFloor);
        if (filter.MaxFloor.HasValue)    q = q.Where(x => x.Floor <= filter.MaxFloor);
        if (filter.HasBalcony.HasValue)  q = q.Where(x => x.HasBalcony == filter.HasBalcony);
        if (filter.HasElevator.HasValue) q = q.Where(x => x.HasElevator == filter.HasElevator);
        if (!string.IsNullOrEmpty(filter.Entrance)) q = q.Where(x => x.Entrance == filter.Entrance);
        if (!string.IsNullOrEmpty(filter.Region))   q = q.Where(x => x.Region.Contains(filter.Region));
        if (!string.IsNullOrEmpty(filter.District)) q = q.Where(x => x.District.Contains(filter.District));
        if (filter.Status.HasValue)      q = q.Where(x => x.Status == filter.Status);

        q = filter.SortBy switch
        {
            "price_asc"  => q.OrderBy(x => x.Price),
            "price_desc" => q.OrderByDescending(x => x.Price),
            "oldest"     => q.OrderBy(x => x.CreatedAt),
            _            => q.OrderByDescending(x => x.CreatedAt)
        };

        var total = q.Count();
        var items = q.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
        var dtos  = _mapper.Map<List<DomApartmentDto>>(items);
        var paged = new PagedResult<DomApartmentDto>
        {
            Items = dtos, TotalCount = total,
            PageNumber = filter.PageNumber, PageSize = filter.PageSize
        };
        return GenericResponse<object>.Success(paged);
    }

    public async Task<GenericResponse<object>> GetByIdAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return GenericResponse<object>.NotFound($"DomApartment {id} not found");
        return GenericResponse<object>.Success(_mapper.Map<DomApartmentDto>(entity));
    }

    public async Task<GenericResponse<object>> CreateAsync(object dtoObj, string sellerId)
    {
        var dto = dtoObj as DomApartmentCreateDto;
        if (dto is null) return GenericResponse<object>.Failure("Invalid DTO");
        var entity = _mapper.Map<DomApartment>(dto);
        entity.SellerId = sellerId;
        await _repo.AddAsync(entity);
        _logger.LogInformation("DomApartment created: {Id}", entity.Id);
        return GenericResponse<object>.Success(_mapper.Map<DomApartmentDto>(entity), "DomApartment created", 201);
    }

    public async Task<GenericResponse<object>> UpdateAsync(Guid id, object dtoObj, string sellerId, bool isAdmin)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return GenericResponse<object>.NotFound();
        if (!isAdmin && entity.SellerId != sellerId) return GenericResponse<object>.Unauthorized();
        _mapper.Map(dtoObj as DomApartmentUpdateDto, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(entity);
        return GenericResponse<object>.Success(_mapper.Map<DomApartmentDto>(entity), "Updated");
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
        return GenericResponse<object>.Success(_mapper.Map<List<DomApartmentDto>>(items));
    }
}
