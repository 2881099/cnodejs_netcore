using System;
using System.Collections.Generic;
using StackExchange.Redis;
using MySql.Data.MySqlClient;

namespace cnodejs.BLL {

	public static partial class RedisHelper {
		public static List<TReturnInfo> ToList<TReturnInfo>(this SelectBuild<TReturnInfo> select, int expireSeconds, string cacheKey = null) { return select.ToList(RedisHelper.Get, RedisHelper.Set, TimeSpan.FromSeconds(expireSeconds), cacheKey); }

		public static string ConnectionString = "";
		public static ConnectionMultiplexer NewConnection() {
			if (string.IsNullOrEmpty(ConnectionString)) {
				string key = "cnodejsRedisConnectionString";
				var ini = IniHelper.LoadIni(@"../web.config");
				if (ini.ContainsKey("connectionStrings")) ConnectionString = ini["connectionStrings"][key];
				if (string.IsNullOrEmpty(ConnectionString)) throw new ArgumentNullException(key, string.Format("未定义 ../web.config 里的 ConnectionStrings 键 '{0}' 或值不正确！", key));
			}
			return ConnectionMultiplexer.Connect(ConnectionString);
		}

		private static ConnectionMultiplexer _connection;
		private static IDatabase _cache;

		public static ConnectionMultiplexer Connection {
			get {
				if (_connection == null || !_connection.IsConnected || _cache == null)
					_connection = NewConnection();
				return _connection;
			}
		}
		public static IDatabase Cache {
			get {
				if (_cache == null || !_cache.IsConnected("test")) {
					_cache = null;
					_cache = Connection.GetDatabase();
				}
				return _cache;
			}
		}
		public static void Set(string key, string value, int expireSeconds = -1) {
			key = string.Concat(Cache.Multiplexer.ClientName, key);
			if (expireSeconds > 0)
				Cache.StringSetAsync(key, value, TimeSpan.FromSeconds(expireSeconds)).Wait();
			else
				Cache.StringSetAsync(key, value).Wait();
		}
		public static string Get(string key) {
			key = string.Concat(Cache.Multiplexer.ClientName, key);
			return Cache.StringGetAsync(key).Result;
		}
		public static void Remove(params string[] key) {
			if (key == null || key.Length == 0) return;
			RedisKey[] rkeys = new RedisKey[key.Length];
			for (int a = 0; a < key.Length; a++) rkeys[a] = string.Concat(Cache.Multiplexer.ClientName, key[a]);
			Cache.KeyDeleteAsync(rkeys).Wait();
		}
		public static bool Exists(string key) {
			key = string.Concat(Cache.Multiplexer.ClientName, key);
			return Cache.KeyExistsAsync(key).Result;
		}
		public static double Increment(string key, double value = 1) {
			key = string.Concat(Cache.Multiplexer.ClientName, key);
			return Cache.StringIncrementAsync(key, value).Result;
		}
	}
}