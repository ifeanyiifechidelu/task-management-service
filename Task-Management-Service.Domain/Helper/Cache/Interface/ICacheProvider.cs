using Microsoft.Extensions.Caching.Distributed;

namespace Task_Management_Service.Domain;

public interface ICacheProvider
{
      Task<T> GetFromCache<T>(string reference);
    Task SetToCache<T>(string reference, T value, TimeSpan absoluteExpireTime);
    Task ClearCache<T>(string reference);
}

