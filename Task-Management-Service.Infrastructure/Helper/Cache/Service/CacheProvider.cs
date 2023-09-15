using Task_Management_Service.Domain;
using MongoDB.Driver;
using Serilog;
using System.Text.Json;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;

namespace Task_Management_Service.Infrastructure;
public class CacheProvider : ICacheProvider
{
    private readonly IDistributedCache _cache;

    public CacheProvider(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetFromCache<T>(string reference)
        {
            Log.Information("Getting from cache...");
            var cacheKey = GetCacheKey<T>(reference);
            var cachedData = await _cache.GetStringAsync(cacheKey);
            return cachedData == null ? default : JsonSerializer.Deserialize<T>(cachedData);
        }

        public async Task SetToCache<T>(string reference, T value, TimeSpan absoluteExpireTime)
        {
            Log.Information("Setting in cache...");
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpireTime };
            var data = JsonSerializer.Serialize(value);
            var cacheKey = GetCacheKey<T>(reference);
            await _cache.SetStringAsync(cacheKey, data , options);
            Log.Information("Cache set completed...");
        }

        public async Task ClearCache<T>(string reference)
        {
            Log.Information("Clearing cache...");
            var cacheKey = GetCacheKey<T>(reference);
            await _cache.RemoveAsync(cacheKey);
        }
        private static string GetCacheKey<T>(string reference)
        {
            return $"{typeof(T).Name}_{reference}_{DateTime.Now:yyyyMMdd_hhmm}";
        }
    }