using CacheMiss.Utility;
using StackExchange.Redis;
using StackExchange.Redis.Profiling;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

/*  ////////////////////////
    Contact with me :
    https://github.com/fTATLISU
    https://www.linkedin.com/in/ferdi-tatlisu-134562a1/
    Created by Ferdi TATLISU
    //////////////////////// */

namespace CacheMiss.Abstracts
{
    public class FakeConnectionMultiplexer : IDisposable, IConnectionMultiplexer
    {
        public string ClientName => FakeKey.I_AM_FAKE_SERVER_KEY;

        public string Configuration => default;

        public int TimeoutMilliseconds => default;

        public long OperationCount => default;

        public bool PreserveAsyncOrder { get => default; set => value = default; }

        public bool IsConnected => false;

        public bool IsConnecting => false;

        public bool IncludeDetailInExceptions { get => default; set => value = default; }
        public int StormLogThreshold { get => default; set => value = default; }

        public event EventHandler<RedisErrorEventArgs> ErrorMessage;
        public event EventHandler<ConnectionFailedEventArgs> ConnectionFailed;
        public event EventHandler<InternalErrorEventArgs> InternalError;
        public event EventHandler<ConnectionFailedEventArgs> ConnectionRestored;
        public event EventHandler<EndPointEventArgs> ConfigurationChanged;
        public event EventHandler<EndPointEventArgs> ConfigurationChangedBroadcast;
        public event EventHandler<HashSlotMovedEventArgs> HashSlotMoved;

        private FakeDatabase _fakeDatabase;
        public FakeConnectionMultiplexer()
        {
            _fakeDatabase = new FakeDatabase(this);
        }

        public void Close(bool allowCommandsToComplete = true)
        {
            
        }

        public Task CloseAsync(bool allowCommandsToComplete = true)
        {
            return Task.CompletedTask;
        }

        public bool Configure(TextWriter log = null)
        {
            return true;
        }

        public async Task<bool> ConfigureAsync(TextWriter log = null)
        {
            return true;
        }

        public void Dispose()
        {
           
        }

        public void ExportConfiguration(Stream destination, ExportOptions options = (ExportOptions)(-1))
        {
            
        }

        public ServerCounters GetCounters()
        {
            return null;
        }

        public IDatabase GetDatabase(int db = -1, object asyncState = null)
        {
            return _fakeDatabase;
        }

        public EndPoint[] GetEndPoints(bool configuredOnly = false)
        {
            return null;
        }

        public int GetHashSlot(RedisKey key)
        {
            return default;
        }

        public IServer GetServer(string host, int port, object asyncState = null)
        {
            return null;
        }

        public IServer GetServer(string hostAndPort, object asyncState = null)
        {
            return null;
        }

        public IServer GetServer(IPAddress host, int port)
        {
            return null;
        }

        public IServer GetServer(EndPoint endpoint, object asyncState = null)
        {
            return null;
        }

        public string GetStatus()
        {
            return FakeKey.I_AM_FAKE_SERVER_KEY;
        }

        public void GetStatus(TextWriter log)
        {
            
        }

        public string GetStormLog()
        {
            return FakeKey.I_AM_FAKE_SERVER_KEY;
        }

        public ISubscriber GetSubscriber(object asyncState = null)
        {
            return null;
        }

        public int HashSlot(RedisKey key)
        {
            return default;
        }

        public long PublishReconfigure(CommandFlags flags = CommandFlags.None)
        {
            return default;
        }

        public async Task<long> PublishReconfigureAsync(CommandFlags flags = CommandFlags.None)
        {
            return default;
        }

        public void RegisterProfiler(Func<ProfilingSession> profilingSessionProvider)
        {
            
        }

        public void ResetStormLog()
        {
            
        }

        public void Wait(Task task)
        {
            
        }

        public T Wait<T>(Task<T> task)
        {
            return default;
        }

        public void WaitAll(params Task[] tasks)
        {
            
        }
    }
}
