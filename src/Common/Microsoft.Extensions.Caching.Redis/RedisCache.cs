﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Microsoft.Extensions.Caching.Redis {
	public class RedisCache : IDistributedCache, IDisposable {
		// KEYS[1] = = key
		// ARGV[1] = absolute-expiration - ticks as long (-1 for none)
		// ARGV[2] = sliding-expiration - ticks as long (-1 for none)
		// ARGV[3] = relative-expiration (long, in seconds, -1 for none) - Min(absolute-expiration - Now, sliding-expiration)
		// ARGV[4] = data - byte[]
		// this order should not change LUA script depends on it
		private const string SetScript = (@"
                redis.call('HMSET', KEYS[1], 'absexp', ARGV[1], 'sldexp', ARGV[2], 'data', ARGV[4])
                if ARGV[3] ~= '-1' then
                  redis.call('EXPIRE', KEYS[1], ARGV[3])
                end
                return 1");
		private const string AbsoluteExpirationKey = "absexp";
		private const string SlidingExpirationKey = "sldexp";
		private const string DataKey = "data";
		private const long NotPresent = -1;

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
			if (optionsAccessor == null) {
				throw new ArgumentNullException(nameof(optionsAccessor));
			}
			_options = optionsAccessor.Value;
			_instance = _options.InstanceName ?? string.Empty;
		}

		public byte[] Get(string key) {
			return GetAndRefresh(key, getData: true);
		}

		public Task<byte[]> GetAsync(string key) {
			return GetAndRefreshAsync(key, getData: true);
		}

		public void Set(string key, byte[] value, DistributedCacheEntryOptions options) {
			this.SetAsync(key, value, options).Wait();
		}

		public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options) {
			if (key == null) {
				throw new ArgumentNullException(nameof(key));
			}
			if (value == null) {
				throw new ArgumentNullException(nameof(value));
			}
			if (options == null) {
				throw new ArgumentNullException(nameof(options));
			}

			var creationTime = DateTimeOffset.UtcNow;
			var absoluteExpiration = GetAbsoluteExpiration(creationTime, options);
			Cache.ScriptEvaluate(SetScript, new RedisKey[] { _instance + key },
				new RedisValue[]
				{
						absoluteExpiration?.Ticks ?? NotPresent,
						options.SlidingExpiration?.Ticks ?? NotPresent,
						GetExpirationInSeconds(creationTime, absoluteExpiration, options) ?? NotPresent,
						value
				});
			return Task.Run(() => { });
		}

		public void Refresh(string key) {
			this.RefreshAsync(key).Wait();
		}

		public Task RefreshAsync(string key) {
			if (key == null) {
				throw new ArgumentNullException(nameof(key));
			}

			return GetAndRefreshAsync(key, getData: false);
		}

		private byte[] GetAndRefresh(string key, bool getData) {
			return this.GetAndRefreshAsync(key, getData).Result;
		}

		private Task<byte[]> GetAndRefreshAsync(string key, bool getData) {
			if (key == null) {
				throw new ArgumentNullException(nameof(key));
			}

			// This also resets the LRU status as desired.
			// TODO: Can this be done in one operation on the server side? Probably, the trick would just be the DateTimeOffset math.
			RedisValue[] results;
			if (getData) {
				results = Cache.HashMemberGet(_instance + key, AbsoluteExpirationKey, SlidingExpirationKey, DataKey);
			} else {
				results = Cache.HashMemberGet(_instance + key, AbsoluteExpirationKey, SlidingExpirationKey);
			}

			// TODO: Error handling
			if (results.Length >= 2) {
				// Note we always get back two results, even if they are all null.
				// These operations will no-op in the null scenario.
				DateTimeOffset? absExpr;
				TimeSpan? sldExpr;
				MapMetadata(results, out absExpr, out sldExpr);
				RefreshAsync(key, absExpr, sldExpr).Wait();
			}

			if (results.Length >= 3 && results[2].HasValue) {
				return Task.FromResult<byte[]>(results[2]);
			}

			return null;
		}

		public void Remove(string key) {
			this.RemoveAsync(key).Wait();
		}

		public Task RemoveAsync(string key) {
			if (key == null) {
				throw new ArgumentNullException(nameof(key));
			}

			Cache.KeyDelete(_instance + key);
			return Task.Run(() => { });
			// TODO: Error handling
		}

		private void MapMetadata(RedisValue[] results, out DateTimeOffset? absoluteExpiration, out TimeSpan? slidingExpiration) {
			absoluteExpiration = null;
			slidingExpiration = null;
			var absoluteExpirationTicks = (long?)results[0];
			if (absoluteExpirationTicks.HasValue && absoluteExpirationTicks.Value != NotPresent) {
				absoluteExpiration = new DateTimeOffset(absoluteExpirationTicks.Value, TimeSpan.Zero);
			}
			var slidingExpirationTicks = (long?)results[1];
			if (slidingExpirationTicks.HasValue && slidingExpirationTicks.Value != NotPresent) {
				slidingExpiration = new TimeSpan(slidingExpirationTicks.Value);
			}
		}

		private void Refresh(string key, DateTimeOffset? absExpr, TimeSpan? sldExpr) {
			this.RefreshAsync(key, absExpr, sldExpr).Wait();
		}

		private Task RefreshAsync(string key, DateTimeOffset? absExpr, TimeSpan? sldExpr) {
			if (key == null) {
				throw new ArgumentNullException(nameof(key));
			}

			// Note Refresh has no effect if there is just an absolute expiration (or neither).
			TimeSpan? expr = null;
			if (sldExpr.HasValue) {
				if (absExpr.HasValue) {
					var relExpr = absExpr.Value - DateTimeOffset.Now;
					expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
				} else {
					expr = sldExpr;
				}
				Cache.KeyExpire(_instance + key, expr);
				// TODO: Error handling
			}
			return Task.Run(() => { });
		}

		private static long? GetExpirationInSeconds(DateTimeOffset creationTime, DateTimeOffset? absoluteExpiration, DistributedCacheEntryOptions options) {
			if (absoluteExpiration.HasValue && options.SlidingExpiration.HasValue) {
				return (long)Math.Min(
					(absoluteExpiration.Value - creationTime).TotalSeconds,
					options.SlidingExpiration.Value.TotalSeconds);
			} else if (absoluteExpiration.HasValue) {
				return (long)(absoluteExpiration.Value - creationTime).TotalSeconds;
			} else if (options.SlidingExpiration.HasValue) {
				return (long)options.SlidingExpiration.Value.TotalSeconds;
			}
			return null;
		}

		private static DateTimeOffset? GetAbsoluteExpiration(DateTimeOffset creationTime, DistributedCacheEntryOptions options) {
			if (options.AbsoluteExpiration.HasValue && options.AbsoluteExpiration <= creationTime) {
				throw new ArgumentOutOfRangeException(
					nameof(DistributedCacheEntryOptions.AbsoluteExpiration),
					options.AbsoluteExpiration.Value,
					"The absolute expiration value must be in the future.");
			}
			var absoluteExpiration = options.AbsoluteExpiration;
			if (options.AbsoluteExpirationRelativeToNow.HasValue) {
				absoluteExpiration = creationTime + options.AbsoluteExpirationRelativeToNow;
			}

			return absoluteExpiration;
		}

		public void Dispose() {
			if (_connection != null) {
				_connection.Close();
			}
		}
	}

	/// <summary>
	/// Configuration options for <see cref="RedisCache"/>.
	/// </summary>
	public class RedisCacheOptions : IOptions<RedisCacheOptions> {
		/// <summary>
		/// The configuration used to connect to Redis.
		/// </summary>
		public string Configuration { get; set; }

		/// <summary>
		/// The Redis instance name.
		/// </summary>
		public string InstanceName { get; set; }

		RedisCacheOptions IOptions<RedisCacheOptions>.Value {
			get { return this; }
		}
	}

	internal static class RedisExtensions {
		private const string HmGetScript = (@"return redis.call('HMGET', KEYS[1], unpack(ARGV))");

		internal static RedisValue[] HashMemberGet(this IDatabase cache, string key, params string[] members) {
			// TODO: Error checking?
			return HashMemberGetAsync(cache, key, members).Result;
		}

		internal static Task<RedisValue[]> HashMemberGetAsync(
			this IDatabase cache,
			string key,
			params string[] members) {
			var result = cache.ScriptEvaluate(
				HmGetScript,
				new RedisKey[] { key },
				GetRedisMembers(members));

			// TODO: Error checking?
			return Task.FromResult((RedisValue[])result);
		}

		private static RedisValue[] GetRedisMembers(params string[] members) {
			var redisMembers = new RedisValue[members.Length];
			for (int i = 0; i < members.Length; i++) {
				redisMembers[i] = (RedisValue)members[i];
			}

			return redisMembers;
		}
	}
}
