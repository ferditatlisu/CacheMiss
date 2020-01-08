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
    public interface ICacheProvider
    {
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        Task<T> Get<T>(string key) where T : class;

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time in seconds</param>
        Task Set<T>(string key, T data, TimeSpan cacheTime) where T : class;

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        Task Remove(string key);
        Task Remove(List<string> keys);

        /// <summary>
        /// Is the value exist with the specified key in the cache
        /// </summary>
        /// <param name="key">/key</param>
        Task<bool> Exist(string key);
    }
}
