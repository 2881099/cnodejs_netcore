using System;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;

namespace cnodejs.BLL {
	/// <summary>
	/// 数据库操作代理类，全部支持走事务
	/// </summary>
	public abstract partial class SqlHelper : cnodejs.DAL.SqlHelper {
	}
}
namespace cnodejs.DAL {
	public abstract partial class SqlHelper {
		private static string _connectionString;
		public static string ConnectionString {
			get {
				if (string.IsNullOrEmpty(_connectionString)) {
					string key = "cnodejsConnectionString";
					var ini = IniHelper.LoadIni(@"../web.config");
					if (ini.ContainsKey("connectionStrings")) _connectionString = ini["connectionStrings"][key];
					//if (string.IsNullOrEmpty(_connectionString)) throw new ArgumentNullException(key, string.Format("未定义 ../web.config 里的 ConnectionStrings 键 '{0}' 或值不正确！", key));
				}
				return _connectionString;
			}
			set {
				_connectionString = value;
				Instance.Pool.ConnectionString = value;
			}
		}
		public static Executer Instance { get; } = new Executer(new LoggerFactory().CreateLogger("cnodejs_DAL_sqlhelper"), ConnectionString);

		public static string Addslashes(string filter, params object[] parms) { return Executer.Addslashes(filter, parms); }
		public static void ExecuteReader(Action<IDataReader> readerHander, string cmdText, params MySqlParameter[] cmdParms) {
			Instance.ExecuteReader(readerHander, CommandType.Text, cmdText, cmdParms);
		}
		public static object[][] ExeucteArray(string cmdText, params MySqlParameter[] cmdParms) {
			return Instance.ExeucteArray(CommandType.Text, cmdText, cmdParms);
		}
		public static int ExecuteNonQuery(string cmdText, params MySqlParameter[] cmdParms) {
			return Instance.ExecuteNonQuery(CommandType.Text, cmdText, cmdParms);
		}
		public static object ExecuteScalar(string cmdText, params MySqlParameter[] cmdParms) {
			return Instance.ExecuteScalar(CommandType.Text, cmdText, cmdParms);
		}
		/// <summary>
		/// 开启事务（不支持异步），10秒未执行完将超时
		/// </summary>
		/// <param name="handler">事务体 () => {}</param>
		public static void Transaction(AnonymousHandler handler) {
			Transaction(handler, TimeSpan.FromSeconds(10));
		}
		/// <summary>
		/// 开启事务（不支持异步）
		/// </summary>
		/// <param name="handler">事务体 () => {}</param>
		/// <param name="timeout">超时</param>
		public static void Transaction(AnonymousHandler handler, TimeSpan timeout) {
			try {
				Instance.BeginTransaction(timeout);
				handler();
				Instance.CommitTransaction();
			} catch (Exception ex) {
				Instance.RollbackTransaction();
				throw ex;
			}
		}
	}
}