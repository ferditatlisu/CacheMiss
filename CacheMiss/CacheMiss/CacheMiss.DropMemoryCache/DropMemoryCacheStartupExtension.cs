using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CacheMiss.DropMemoryCache
{
    public static class DropMemoryCacheStartupExtension
    {
        public static void AddCacheMissDropMemoryCacheService(this IServiceCollection services, string redisConnection)
        {
            services.AddMemoryCache();
            services.AddSingleton<ConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
            services.AddSingleton<IDropCacheService, DropCacheService>();
        }
    }
}
