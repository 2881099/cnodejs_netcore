using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.DAL {

	public partial class Users_topics : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`users_topics`";
			internal static readonly string Field = "a.`topics_id`, a.`users_id`";
			internal static readonly string Sort = "a.`topics_id`, a.`users_id`";
			public static readonly string Delete = "DELETE FROM `users_topics` WHERE ";
			public static readonly string Insert = "INSERT INTO `users_topics`(`topics_id`, `users_id`) VALUES(?topics_id, ?users_id)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Users_topicsInfo item) {
			return new MySqlParameter[] {
				GetParameter("?topics_id", MySqlDbType.UInt64, 20, item.Topics_id), 
				GetParameter("?users_id", MySqlDbType.UInt64, 20, item.Users_id)};
		}
		public Users_topicsInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Users_topicsInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new Users_topicsInfo {
				Topics_id = dr.IsDBNull(++index) ? null : (ulong?)dr.GetInt64(index), 
				Users_id = dr.IsDBNull(++index) ? null : (ulong?)dr.GetInt64(index)};
		}
		public SelectBuild<Users_topicsInfo> Select {
			get { return SelectBuild<Users_topicsInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(ulong? Topics_id, ulong? Users_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`topics_id` = ?topics_id AND `users_id` = ?users_id"), 
				GetParameter("?topics_id", MySqlDbType.UInt64, 20, Topics_id), 
				GetParameter("?users_id", MySqlDbType.UInt64, 20, Users_id));
		}
		public int DeleteByTopics_id(ulong? Topics_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`topics_id` = ?topics_id"), 
				GetParameter("?topics_id", MySqlDbType.UInt64, 20, Topics_id));
		}
		public int DeleteByUsers_id(ulong? Users_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`users_id` = ?users_id"), 
				GetParameter("?users_id", MySqlDbType.UInt64, 20, Users_id));
		}

		public int Update(Users_topicsInfo item) {
			return new SqlUpdateBuild(null, item.Topics_id, item.Users_id).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Users_topicsInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Users_topicsInfo item, ulong? Topics_id, ulong? Users_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`topics_id` = {0} AND `users_id` = {1}", Topics_id, Users_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 cnodejs.DAL.Users_topics.SqlUpdateBuild 误修改，请必须设置 where 条件。");
				return string.Concat("UPDATE ", TSQL.Table, " SET ", _fields.Substring(1), " WHERE ", _where);
			}
			public int ExecuteNonQuery() {
				string sql = this.ToString();
				if (string.IsNullOrEmpty(sql)) return 0;
				return SqlHelper.ExecuteNonQuery(sql, _parameters.ToArray());
			}
			public SqlUpdateBuild Where(string filterFormat, params object[] values) {
				if (!string.IsNullOrEmpty(_where)) _where = string.Concat(_where, " AND ");
				_where = string.Concat(_where, "(", SqlHelper.Addslashes(filterFormat, values), ")");
				return this;
			}
			public SqlUpdateBuild Set(string field, string value, params MySqlParameter[] parms) {
				if (value.IndexOf('\'') != -1) throw new Exception("cnodejs.DAL.Users_topics.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
		}
		#endregion

		public Users_topicsInfo Insert(Users_topicsInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public Users_topicsInfo GetItem(ulong? Topics_id, ulong? Users_id) {
			return this.Select.Where("a.`topics_id` = {0} AND a.`users_id` = {1}", Topics_id, Users_id).ToOne();
		}
	}
}