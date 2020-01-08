using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/*  ////////////////////////
    Contact with me :
    https://github.com/fTATLISU
    https://www.linkedin.com/in/ferdi-tatlisu-134562a1/
    Created by Ferdi TATLISU
    //////////////////////// */

namespace CacheMiss
{
    public static class CacheExtensions
    {
        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="key">Cache key</param>
        /// <param name="cacheTime">Cache time in minutes (0 - do not cache)</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>Cached item</returns>
        public static async Task<T> Get<T>(this ICacheProvider cacheManager, string key, TimeSpan cacheTime, Func<string, TimeSpan, Task<T>> acquire) where T : class
        {
            T result = null;
            bool isError = false;
            try
            {
                result = await cacheManager.Get<T>(key);
                if (result == null)
                {
                    result = await acquire(key, cacheTime);
                    if (cacheTime.TotalSeconds > 0 && result != null)
                    {
                        await cacheManager.Set(key, result, cacheTime);
                    }
                }
            }
            catch (Exception e)
            {
                if(CacheMissSettings.DeveloperMode)
                    throw e;

                isError = true;
            }

            if (isError && !(cacheManager is LocalCacheProvider))
            {
                result = await acquire(key, cacheTime);
            }

            return result;
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="pattern">Pattern</param>
        /// <param name="keys">All keys in the cache</param>
        public static void RemoveByPattern(this ICacheProvider cacheManager, string pattern, IEnumerable<string> keys)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (var key in keys.Where(p => regex.IsMatch(p.ToString())).ToList())
                cacheManager.Remove(key);
        }

        public static async Task<bool> Exist(this ICacheProvider cacheManager, string key, Func<string, Task<bool>> acquire)
        {
            bool isExist = await cacheManager.Exist(key);
            if (!isExist)
            {
                isExist = await acquire(key);
            }

            return isExist;
        }
    }
}