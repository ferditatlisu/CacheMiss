using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*  ////////////////////////
    Contact with me :
    https://github.com/fTATLISU
    https://www.linkedin.com/in/ferdi-tatlisu-134562a1/
    Created by Ferdi TATLISU
    //////////////////////// */

namespace CacheMiss
{
    public class RedisProvider : ICacheProvider, IDisposable
    {
        #region Field

        protected RedisCacheManager _RedisCacheManager;

        #endregion

        #region Property

        #endregion

        #region Public Method

        public RedisProvider(RedisCacheManager cacheManager)
        {
            _RedisCacheManager = cacheManager;
        }

        public async Task<T> Get<T>(string key) where T : class
        {
            var data = _RedisCacheManager.DeSerializeData<T>(await _RedisCacheManager.Database.StringGetAsync(key));
            return data;
        }

        public async Task<bool> Exist(string key)
        {
            try
            {
                var isExist = await _RedisCacheManager.Database.KeyExistsAsync(key);
                return isExist;
            }
            catch (Exception e)
            {
                if (CacheMissSettings.DeveloperMode)
                    throw;
            }

            return false;
        }

        public async Task Remove(string key)
        {
            try
            {
                await _RedisCacheManager.Database.KeyDeleteAsync(key);
            }
            catch (Exception e)
            {
                if (CacheMissSettings.DeveloperMode)
                    throw;
            }
        }

        public async Task Remove(List<string> keys)
        {
            try
            {
                RedisKey[] redisKeys = keys.Select(y => (RedisKey)y).ToArray();
                await _RedisCacheManager.Database.KeyDeleteAsync(redisKeys);
            }
            catch (Exception e)
            {
                if (CacheMissSettings.DeveloperMode)
                    throw;
            }
        }

        public async Task Set<T>(string key, T data, TimeSpan cacheTime) where T : class
        {
            if (data == null)
                return;

            await _RedisCacheManager.Database.StringSetAsync(key, _RedisCacheManager.SerializeData(data), cacheTime);
        }

        public void Dispose()
        {
            _RedisCacheManager.Dispose();
        }

        #endregion

        #region Private-Helper

        #endregion
    }
}
