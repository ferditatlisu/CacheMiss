using StackExchange.Redis;
using System;

/*  ////////////////////////
    Contact with me :
    https://github.com/fTATLISU
    https://www.linkedin.com/in/ferdi-tatlisu-134562a1/
    Created by Ferdi TATLISU
    //////////////////////// */

namespace CacheMiss.Abstracts
{
    public class CacheMissConnectionMultiplexer : IDisposable
    {
        #region Field

        public readonly FakeConnectionMultiplexer FakeConnectionMultiplexer;
        public readonly ConnectionMultiplexer Connection;

        #endregion

        #region Property

        public bool IsConnected { get; set; }


        #endregion

        #region Public Method

        public CacheMissConnectionMultiplexer(ConfigurationOptions options)
        {
            try
            {
                Connection = ConnectionMultiplexer.ConnectAsync(options).Result;
                IsConnected = true;
            }
            catch
            {
                if (!CacheMissSettings.FakeConnection)
                    throw;

                FakeConnectionMultiplexer = new FakeConnectionMultiplexer();
                IsConnected = false;
            }
        }

        public IDatabase GetDatabase()
        {
            if (IsConnected)
                return Connection.GetDatabase();
            else
                return FakeConnectionMultiplexer.GetDatabase();
        }

        public void Dispose()
        {
            FakeConnectionMultiplexer?.Dispose();
            Connection?.Dispose();
        }

        #endregion

        #region Private-Helper

        #endregion
    }
}

