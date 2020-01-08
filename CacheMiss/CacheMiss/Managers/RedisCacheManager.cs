using CacheMiss.Abstracts;
using ProtoBuf;
using StackExchange.Redis;
using System;
using System.IO;
using System.IO.Compression;

/*  ////////////////////////
    Contact with me :
    https://github.com/fTATLISU
    https://www.linkedin.com/in/ferdi-tatlisu-134562a1/
    Created by Ferdi TATLISU
    //////////////////////// */

namespace CacheMiss
{
    public class RedisCacheManager
    {
        #region Field

        private readonly Lazy<CacheMissConnectionMultiplexer> _lazyConnection;

        #endregion

        #region Property

        public CacheMissConnectionMultiplexer Connection
        {
            get
            {
                return _lazyConnection.Value;
            }
        }

        public IDatabase Database
        {
            get { return  Connection.GetDatabase(); }
        }

        #endregion

        #region Public Method

        public RedisCacheManager(string connectionString)
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(connectionString);
            options.AllowAdmin = true;
            _lazyConnection = new Lazy<CacheMissConnectionMultiplexer>(() =>
            {
                CacheMissConnectionMultiplexer multiplexer = new CacheMissConnectionMultiplexer(options);
                return multiplexer;
            });
        }

        public byte[] SerializeData<T>(T data, bool zip = false)
        {
            if (data != null)
            {
                if (zip)
                {
                    using var ms = new MemoryStream();
                    using (GZipStream gzip = new GZipStream(ms, CompressionMode.Compress, true))
                    using (var bs = new BufferedStream(gzip, 64 * 1024))
                    {
                        Serializer.Serialize(bs, data);
                    }

                    return ms.ToArray();
                }
                else
                {
                    using var stream = new MemoryStream();
                    Serializer.Serialize(stream, data);
                    return stream.ToArray();
                }
            }
            else
            {
                return null;
            }
        }

        public T DeSerializeData<T>(RedisValue data, bool zip = false)
        {
            if (!data.IsNullOrEmpty)
            {
                if (zip)
                {
                    using var ms = new MemoryStream(data);
                    using var gzip = new GZipStream(ms, CompressionMode.Decompress, true);
                    return Serializer.Deserialize<T>(gzip);
                }
                else
                {
                    using (var stream = new MemoryStream(data))
                    {
                        return Serializer.Deserialize<T>(stream);
                    };
                }
            }
            else
            {
                return default;
            }
        }

        public T DeSerializeData<T>(byte[] data, bool zip = false)
        {
            if (data != null && data.Length > 0)
            {
                if (zip)
                {
                    using var ms = new MemoryStream(data);
                    using var gzip = new GZipStream(ms, CompressionMode.Decompress, true);
                    return Serializer.Deserialize<T>(gzip);
                }
                else
                {
                    using (var stream = new MemoryStream(data))
                    {
                        return Serializer.Deserialize<T>(stream);
                    };
                }
            }
            else
            {
                return default;
            }
        }

        #endregion

        #region Private-Helper

        #endregion
    }
}
