using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Microsoft.Extensions.Caching.Redis {
	public class RedisCache : IDistributedCache, IDisposable {
		private ConnectionMultiplexer _connection;
		private IDatabase _cache;

		public ConnectionMultiplexer Connection {
			get {
				if (_connection == null || !_connection.IsConnected || _cache == null)
					_connection = ConnectionMultiplexer.Connect(_options.Configuration);
				return _connection;
			}
		}
		public IDatabase Cache {
			get {
				if (_cache == null || !_cache.IsConnected("test")) {
					_cache = null;
					_cache = Connection.GetDatabase();
				}
				return _cache;
			}
		}

		private readonly RedisCacheOptions _options;
		private readonly string _instance;

		public RedisCache(IOptions<RedisCacheOptions> optionsAccessor) {
			if (optionsAccessor == null) throw new ArgumentNullException(nameof(optionsAccessor));
			_options = optionsAccessor.Value;
			_instance = _options.InstanceName ?? "session";
		}

		public byte[] Get(string key) {
			return this.GetAsync(key).Result;
		}
		public Task<byte[]> GetAsync(string key) {
			if (key == null) throw new ArgumentNullException(nameof(key));

			var hkey = _instance + key;
			var ret = Cache.HashGet(hkey, "data");
			return Task.FromResult<byte[]>(ret);
		}

		public void Set(string key, byte[] value, DistributedCacheEntryOptions options) {
			this.SetAsync(key, value, options).Wait();
		}
		public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options) {
			if (key == null) throw new ArgumentNullException(nameof(key));
			if (value == null) throw new ArgumentNullException(nameof(value));
			if (options == null) throw new ArgumentNullException(nameof(options));

			var hkey = _instance + key;
			var expire = options.AbsoluteExpiration.HasValue ? options.AbsoluteExpirationRelativeToNow.Value : options.SlidingExpiration ?? TimeSpan.FromMinutes(20);
			Cache.HashSet(hkey, new HashEntry[] {
				new HashEntry("expire", expire.Ticks),
				new HashEntry("data", value)
			});
			Cache.KeyExpire(hkey, expire);
			return Task.Run(() => { });
		}

		public void Refresh(string key) {
			this.RefreshAsync(key).Wait();
		}

		public Task RefreshAsync(string key) {
			if (key == null) throw new ArgumentNullException(nameof(key));

			var hkey = _instance + key;
			long expire;
			if (Cache.HashGet(hkey, "expire").TryParse(out expire) && expire > 0) Cache.KeyExpire(hkey, TimeSpan.FromTicks(expire));
			return Task.Run(() => { });
		}
		public void Remove(string key) {
			this.RemoveAsync(key).Wait();
		}

		public Task RemoveAsync(string key) {
			if (key == null) throw new ArgumentNullException(nameof(key));

			var hkey = _instance + key;
			Cache.KeyDelete(hkey);
			return Task.Run(() => { });
		}

		public void Dispose() {
			if (_connection != null) {
				_connection.Close();
			}
		}
	}

	public class RedisCacheOptions : IOptions<RedisCacheOptions> {
		public string Configuration { get; set; }
		public string InstanceName { get; set; }

		RedisCacheOptions IOptions<RedisCacheOptions>.Value {
			get { return this; }
		}
	}
}
