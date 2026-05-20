using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstate.Application.DTOs.Filter;
using RealEstate.Application.DTOs.Property;
using RealEstate.Domain.Common;
using RealEstate.Domain.Entities.Property;
using RealEstate.Domain.Enums;
using RealEstate.Domain.Interfaces.Repositories;
using RealEstate.Domain.Interfaces.Services;

namespace RealEstate.Application.Services;

public class HavliService : IHavliService
{
    private readonly IHavliRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<HavliService> _logger;

    public HavliService(IHavliRepository repo, IMapper mapper, ILogger<HavliService> logger)
    {
        _repo = repo; _mapper = mapper; _logger = logger;
    }

    public async Task<GenericResponse<object>> GetAllAsync(object filterObj)
    {
        var filter = filterObj as HavliFilterDto ?? new HavliFilterDto();
        var all = await _repo.GetAllAsync();
        var q = all.AsQueryable();

        if (filter.MinPrice.HasValue)  q = q.Where(x => x.Price >= filter.MinPrice);
        if (filter.MaxPrice.HasValue)  q = q.Where(x => x.Price <= filter.MaxPrice);
        if (filter.MinArea.HasValue)   q = q.Where(x => x.Area >= filter.MinArea);
        if (filter.MaxArea.HasValue)   q = q.Where(x => x.Area <= filter.MaxArea);
        if (filter.MinRooms.HasValue)  q = q.Where(x => x.Rooms >= filter.MinRooms);
        if (filter.MaxRooms.HasValue)  q = q.Where(x => x.Rooms <= filter.MaxRooms);
        if (filter.HasGarage.HasValue) q = q.Where(x => x.HasGarage == filter.HasGarage);
        if (filter.HasPool.HasValue)   q = q.Where(x => x.HasPool == filter.HasPool);
        if (!string.IsNullOrEmpty(filter.Region))   q = q.Where(x => x.Region.Contains(filter.Region));
        if (!string.IsNullOrEmpty(filter.District)) q = q.Where(x => x.District.Contains(filter.District));
        if (filter.Status.HasValue)    q = q.Where(x => x.Status == filter.Status);

        q = filter.SortBy switch
        {
            "price_asc"  => q.OrderBy(x => x.Price),
            "price_desc" => q.OrderByDescending(x => x.Price),
            "oldest"     => q.OrderBy(x => x.CreatedAt),
            _            => q.OrderByDescending(x => x.CreatedAt)
        };

        var total = q.Count();
        var items = q.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
        var dtos  = _mapper.Map<List<HavliDto>>(items);

        var paged = new PagedResult<HavliDto>
        {
            Items = dtos, TotalCount = total,
            PageNumber = filter.PageNumber, PageSize = filter.PageSize
        };
        return GenericResponse<object>.Success(paged);
    }

    public async Task<GenericResponse<object>> GetByIdAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return GenericResponse<object>.NotFound($"Havli with id {id} not found");
        return GenericResponse<object>.Success(_mapper.Map<HavliDto>(entity));
    }

    public async Task<GenericResponse<object>> CreateAsync(object dtoObj, string sellerId)
    {
        var dto = dtoObj as HavliCreateDto;
        if (dto is null) return GenericResponse<object>.Failure("Invalid DTO");
        var entity = _mapper.Map<Havli>(dto);
        entity.SellerId = sellerId;
        await _repo.AddAsync(entity);
        _logger.LogInformation("Havli created: {Id} by seller {SellerId}", entity.Id, sellerId);
        return GenericResponse<object>.Success(_mapper.Map<HavliDto>(entity), "Havli created successfully", 201);
    }

    public async Task<GenericResponse<object>> UpdateAsync(Guid id, object dtoObj, string sellerId, bool isAdmin)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return GenericResponse<object>.NotFound();
        if (!isAdmin && entity.SellerId != sellerId) return GenericResponse<object>.Unauthorized("You can only update your own listings");
        var dto = dtoObj as HavliUpdateDto;
        if (dto is null) return GenericResponse<object>.Failure("Invalid DTO");
        _mapper.Map(dto, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(entity);
        return GenericResponse<object>.Success(_mapper.Map<HavliDto>(entity), "Havli updated");
    }

    public async Task<GenericResponse<object>> DeleteAsync(Guid id, string sellerId, bool isAdmin)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return GenericResponse<object>.NotFound();
        if (!isAdmin && entity.SellerId != sellerId) return GenericResponse<object>.Unauthorized("You can only delete your own listings");
        await _repo.DeleteAsync(id);
        return GenericResponse<object>.Success(new { }, "Havli deleted");
    }

    public async Task<GenericResponse<object>> GetMyListingsAsync(string sellerId)
    {
        var items = await _repo.GetBySellerIdAsync(sellerId);
        return GenericResponse<object>.Success(_mapper.Map<List<HavliDto>>(items));
    }
}
