using System;
using System.Collections.Generic;
using System.Threading;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace MySql.Data.MySqlClient {
	/// <summary>
	/// 数据库链接池
	/// </summary>
	public partial class ConnectionPool {

		public int MaxPoolSize = 32;
		public List<SqlConnection2> AllConnections = new List<SqlConnection2>();
		public Queue<SqlConnection2> FreeConnections = new Queue<SqlConnection2>();
		public Queue<ManualResetEvent> GetConnectionQueue = new Queue<ManualResetEvent>();
		private static object _lock = new object();
		private static object _lock_GetConnectionQueue = new object();
		private string _connectionString;
		public string ConnectionString {
			get { return _connectionString; }
			set {
				_connectionString = value;
				Match m = Regex.Match(_connectionString, @"Max\s*pool\s*size=(\d+)", RegexOptions.IgnoreCase);
				if (m.Success) int.TryParse(m.Groups[1].Value, out MaxPoolSize);
				else MaxPoolSize = 32;
				if (MaxPoolSize <= 0) MaxPoolSize = 32;
			}
		}

		public ConnectionPool(string connectionString) {
			ConnectionString = connectionString;
		}

		public SqlConnection2 GetConnection() {
			SqlConnection2 conn = null;
			int tid = Thread.CurrentThread.ManagedThreadId;

			if (FreeConnections.Count > 0)
				lock (_lock)
					conn = FreeConnections.Dequeue();
			if (conn == null && AllConnections.Count < MaxPoolSize) {
				conn = new SqlConnection2 {
					ThreadId = tid,
					SqlConnection = new MySqlConnection(ConnectionString)
				};
				lock (_lock)
					AllConnections.Add(conn);
			}
			if (conn == null) {
				ManualResetEvent wait = new ManualResetEvent(false);
				lock (_lock_GetConnectionQueue)
					GetConnectionQueue.Enqueue(wait);
				if (wait.WaitOne(TimeSpan.FromSeconds(10)))
					return GetConnection();
				return null;
			}
			conn.ThreadId = tid;
			conn.LastActive = DateTime.Now;
			Interlocked.Increment(ref conn.UseSum);
			return conn;
		}

		public void ReleaseConnection(SqlConnection2 conn) {
			conn.SqlConnection.Close();
			lock (_lock)
				FreeConnections.Enqueue(conn);

			if (GetConnectionQueue.Count > 0) {
				ManualResetEvent wait = null;
				lock (_lock_GetConnectionQueue)
					wait = GetConnectionQueue.Dequeue();
				if (wait != null) wait.Set();
			}
		}
	}

	public class SqlConnection2 {
		public MySqlConnection SqlConnection;
		public DateTime LastActive;
		public long UseSum;
		internal int ThreadId;
	}
}