using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CacheMiss.DropMemoryCache
{
    public interface IDropCacheService
    {
        void Subscribe();
        Task Drop(string cacheKey);
    }
}
