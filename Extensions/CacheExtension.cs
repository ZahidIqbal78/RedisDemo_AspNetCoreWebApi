using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace RedisDemo_AspNetCoreWebApi.Extensions
{
    public static class CacheExtension
    {
        public static async Task SetCacheRecordAsync<T>(this IDistributedCache cache,
            string recordId,
            T data,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            options.SlidingExpiration = unusedExpireTime;

            //serialize the data
            var jsonData = JsonSerializer.Serialize(data);
            //save/cache the serialized data
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetCacheRecordAsync<T>(this IDistributedCache cache,
            string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);
            if(jsonData is null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
