using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.DAL {

	public partial class Roles_users : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`roles_users`";
			internal static readonly string Field = "a.`roles_id`, a.`users_id`";
			internal static readonly string Sort = "a.`roles_id`, a.`users_id`";
			public static readonly string Delete = "DELETE FROM `roles_users` WHERE ";
			public static readonly string Insert = "INSERT INTO `roles_users`(`roles_id`, `users_id`) VALUES(?roles_id, ?users_id)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Roles_usersInfo item) {
			return new MySqlParameter[] {
				GetParameter("?roles_id", MySqlDbType.UInt32, 10, item.Roles_id), 
				GetParameter("?users_id", MySqlDbType.UInt64, 20, item.Users_id)};
		}
		public Roles_usersInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Roles_usersInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new Roles_usersInfo {
				Roles_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Users_id = dr.IsDBNull(++index) ? null : (ulong?)dr.GetInt64(index)};
		}
		public SelectBuild<Roles_usersInfo> Select {
			get { return SelectBuild<Roles_usersInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Roles_id, ulong? Users_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`roles_id` = ?roles_id AND `users_id` = ?users_id"), 
				GetParameter("?roles_id", MySqlDbType.UInt32, 10, Roles_id), 
				GetParameter("?users_id", MySqlDbType.UInt64, 20, Users_id));
		}
		public int DeleteByUsers_id(ulong? Users_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`users_id` = ?users_id"), 
				GetParameter("?users_id", MySqlDbType.UInt64, 20, Users_id));
		}
		public int DeleteByRoles_id(uint? Roles_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`roles_id` = ?roles_id"), 
				GetParameter("?roles_id", MySqlDbType.UInt32, 10, Roles_id));
		}

		public int Update(Roles_usersInfo item) {
			return new SqlUpdateBuild(null, item.Roles_id, item.Users_id).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Roles_usersInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Roles_usersInfo item, uint? Roles_id, ulong? Users_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`roles_id` = {0} AND `users_id` = {1}", Roles_id, Users_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 cnodejs.DAL.Roles_users.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("cnodejs.DAL.Roles_users.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
		}
		#endregion

		public Roles_usersInfo Insert(Roles_usersInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public Roles_usersInfo GetItem(uint? Roles_id, ulong? Users_id) {
			return this.Select.Where("a.`roles_id` = {0} AND a.`users_id` = {1}", Roles_id, Users_id).ToOne();
		}
	}
}