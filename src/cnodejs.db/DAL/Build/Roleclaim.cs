using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.DAL {

	public partial class Roleclaim : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`roleclaim`";
			internal static readonly string Field = "a.`id`, a.`roles_id`, a.`create_time`, a.`type`, a.`value`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `roleclaim` WHERE ";
			public static readonly string Insert = "INSERT INTO `roleclaim`(`roles_id`, `create_time`, `type`, `value`) VALUES(?roles_id, ?create_time, ?type, ?value); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(RoleclaimInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?roles_id", MySqlDbType.UInt32, 10, item.Roles_id), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?type", MySqlDbType.VarChar, 96, item.Type), 
				GetParameter("?value", MySqlDbType.VarChar, 255, item.Value)};
		}
		public RoleclaimInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as RoleclaimInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new RoleclaimInfo {
				Id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Roles_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Create_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Type = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Value = dr.IsDBNull(++index) ? null : (string)dr.GetString(index)};
		}
		public SelectBuild<RoleclaimInfo> Select {
			get { return SelectBuild<RoleclaimInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByRoles_id(uint? Roles_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`roles_id` = ?roles_id"), 
				GetParameter("?roles_id", MySqlDbType.UInt32, 10, Roles_id));
		}

		public int Update(RoleclaimInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetRoles_id(item.Roles_id)
				.SetCreate_time(item.Create_time)
				.SetType(item.Type)
				.SetValue(item.Value).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected RoleclaimInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(RoleclaimInfo item, uint? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 cnodejs.DAL.Roleclaim.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("cnodejs.DAL.Roleclaim.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetRoles_id(uint? value) {
				if (_item != null) _item.Roles_id = value;
				return this.Set("`roles_id`", string.Concat("?roles_id_", _parameters.Count), 
					GetParameter(string.Concat("?roles_id_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", string.Concat("?create_time_", _parameters.Count), 
					GetParameter(string.Concat("?create_time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetType(string value) {
				if (_item != null) _item.Type = value;
				return this.Set("`type`", string.Concat("?type_", _parameters.Count), 
					GetParameter(string.Concat("?type_", _parameters.Count), MySqlDbType.VarChar, 96, value));
			}
			public SqlUpdateBuild SetValue(string value) {
				if (_item != null) _item.Value = value;
				return this.Set("`value`", string.Concat("?value_", _parameters.Count), 
					GetParameter(string.Concat("?value_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
		}
		#endregion

		public RoleclaimInfo Insert(RoleclaimInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public RoleclaimInfo GetItem(uint? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
	}
}