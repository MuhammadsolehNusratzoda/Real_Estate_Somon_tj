using RealEstate.Domain.Common;

namespace RealEstate.Domain.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<PagedResult<T>> GetPagedAsync(int page, int pageSize);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
