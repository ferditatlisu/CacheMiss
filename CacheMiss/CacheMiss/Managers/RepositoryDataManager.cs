using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

/*  ////////////////////////
    Contact with me :
    https://github.com/fTATLISU
    https://www.linkedin.com/in/ferdi-tatlisu-134562a1/
    Created by Ferdi TATLISU
    //////////////////////// */

namespace CacheMiss
{   
    [CacheMissAttribute]
    public class RepositoryDataManager<T> : IRepositoryDataManager where T : DbContext
    {
        #region Field

        protected ICacheProvider _RedisProvider;
        protected ICacheProvider _LocalCacheProvider;
        protected T _DbContext;

        #endregion

        #region Property

        #endregion

        #region Public Method

        public RepositoryDataManager(ICacheProvider memoryProvider, ICacheProvider redisProvider, T dbContext)
        {
            _LocalCacheProvider = memoryProvider;
            _RedisProvider = redisProvider;
            _DbContext = dbContext;
        }

        #endregion

        #region SP

        public async Task<List<TModel>> ExecuteSQL<TModel>(string query, SqlParameter[] param)
        {
            using var command = _DbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(param);
            await _DbContext.Database.OpenConnectionAsync();
            List<TModel> list = new List<TModel>();
            using (var result = await command.ExecuteReaderAsync())
            {
                var r = Serialize((SqlDataReader)result);
                string json = JsonConvert.SerializeObject(r, Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<TModel>>(json);
            }

            _DbContext.Database.CloseConnection();
            return list;
        }

        private IEnumerable<Dictionary<string, object>> Serialize(SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                cols.Add(reader.GetName(i));
            }

            while (reader.Read())
            {
                results.Add(SerializeRow(cols, reader));
            }

            return results;
        }

        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols, SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
            {
                result.Add(col, reader[col]);
            }

            return result;
        }

        #endregion

        #region Private-Helper

        #endregion
    }
}
