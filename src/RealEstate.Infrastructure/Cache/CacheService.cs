using Microsoft.Extensions.Caching.Memory;

namespace RealEstate.Infrastructure.Cache;

public class CacheService
{
    private readonly IMemoryCache _cache;

    public CacheService(IMemoryCache cache) => _cache = cache;

    public T? Get<T>(string key) => _cache.TryGetValue(key, out T? value) ? value : default;

    public void Set<T>(string key, T value, TimeSpan? expiry = null)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(10)
        };
        _cache.Set(key, value, options);
    }

    public void Remove(string key) => _cache.Remove(key);

    public bool TryGetValue<T>(string key, out T? value) => _cache.TryGetValue(key, out value);
}
