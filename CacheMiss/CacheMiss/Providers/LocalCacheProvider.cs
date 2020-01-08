using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/*  ////////////////////////
    Contact with me :
    https://github.com/fTATLISU
    https://www.linkedin.com/in/ferdi-tatlisu-134562a1/
    Created by Ferdi TATLISU
    //////////////////////// */

namespace CacheMiss
{
    public class LocalCacheProvider : ICacheProvider
    {
        #region Field

        #endregion

        #region Property

        #endregion

        #region Public Method

        private MemoryCache _Cache;

        public LocalCacheProvider()
        {
            MemoryCacheOptions options = new MemoryCacheOptions();
            _Cache = new MemoryCache(options);
        }

        public async Task<bool> Exist(string key)
        {
            bool isExist = false;
            var data = _Cache.Get(key);
            if (data != null)
            {
                isExist = true;
            }

            return isExist;
        }

        public async Task<T> Get<T>(string key) where T : class
        {
            var data = _Cache.Get<T>(key) as T;
            return data;
        }

        public async Task Remove(string key)
        {
            _Cache.Remove(key);
        }

        public async Task Remove(List<string> keys)
        {
            foreach (var key in keys)
            {
                _Cache. Remove(key);
            }
        }

        public async Task Set<T>(string key, T data, TimeSpan cacheTime) where T : class
        {
            if (data == null)
                return;

            MemoryCacheEntryOptions option = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.UtcNow + cacheTime
            };

            _Cache.Set(key, data, option);
        }

        #endregion

        #region Private-Helper

        #endregion
    }
}
