using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace CacheMiss.DropMemoryCache
{
    public class DropCacheService : IDropCacheService
    {
        public const string DROP_CACHE_CHANNEL_NAME = "DROP_CACHE_CHANNEL_NAME";

        private readonly IMemoryCache _memoryCache;
        private readonly ConnectionMultiplexer _redisMultiplexer;
        private readonly ILogger _logger;

        public DropCacheService(IMemoryCache memoryCache, ConnectionMultiplexer redisMultiplexer, ILogger<DropCacheService> logger)
        {
            _memoryCache = memoryCache;
            _redisMultiplexer = redisMultiplexer;
            _logger = logger;
        }

        public async Task Drop(string cacheKey)
        {
            await _redisMultiplexer.GetSubscriber().PublishAsync(DROP_CACHE_CHANNEL_NAME, cacheKey);
        }

        public void Subscribe()
        {
            _logger.LogInformation("Listener subscribed");
            _redisMultiplexer.GetSubscriber().Subscribe(DROP_CACHE_CHANNEL_NAME,
                (channel, message) => Listener(channel, message));
        }

        private void Listener(RedisChannel _, RedisValue message)
        {
            _logger.LogInformation($"Listener called and {message} removed from memory");
            var stringKey = message.ToString();
            _memoryCache.Remove(stringKey);
        }
    }
}
