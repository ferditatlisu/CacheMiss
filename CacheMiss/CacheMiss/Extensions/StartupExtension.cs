using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

/*  ////////////////////////
    Contact with me :
    https://github.com/fTATLISU
    https://www.linkedin.com/in/ferdi-tatlisu-134562a1/
    Created by Ferdi TATLISU
    //////////////////////// */

namespace CacheMiss
{
    public static class StartupExtension
    {
        #region Field

        #endregion

        #region Property

        #endregion

        #region Public Method

        public static void AddCacheMiss<T>(this IServiceCollection services, string dbConnection, string redisConnection) where T: DbContext
        {
            services.AddTransient<LocalCacheProvider>();
            services.AddTransient(_ => new RedisProvider(new RedisCacheManager(redisConnection)));
            services.AddDbContextPool<T>(options => options.UseSqlServer(dbConnection));

            var typesWithMyAttribute =
            from a in System.AppDomain.CurrentDomain.GetAssemblies()
            from t in a.GetTypes()
            let attributes = t.GetCustomAttributes(typeof(CacheMissAttribute), true)
            where attributes != null && attributes.Length > 0
            select new { Type = t, Attributes = attributes.Cast<CacheMissAttribute>() };
            var aa = typesWithMyAttribute.Where(z => z.Type.BaseType?.Name == "RepositoryDataManager`1" || z.Type.BaseType?.Name == "RepositoryDataManager").Select(y => y.Type).ToList();
            foreach (var item in aa)
            {
                services.AddScoped(item);
            }
        }

        #endregion

        #region Private-Helper

        #endregion
    }
}