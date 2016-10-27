using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.DAL {

	public partial class Sysdoc : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`sysdoc`";
			internal static readonly string Field = "a.`id`, a.`content`, a.`create_time`, a.`title`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `sysdoc` WHERE ";
			public static readonly string Insert = "INSERT INTO `sysdoc`(`content`, `create_time`, `title`) VALUES(?content, ?create_time, ?title); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(SysdocInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?content", MySqlDbType.Text, -1, item.Content), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?title", MySqlDbType.VarChar, 255, item.Title)};
		}
		public SysdocInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as SysdocInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new SysdocInfo {
				Id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Content = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Create_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Title = dr.IsDBNull(++index) ? null : (string)dr.GetString(index)};
		}
		public SelectBuild<SysdocInfo> Select {
			get { return SelectBuild<SysdocInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}

		public int Update(SysdocInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetContent(item.Content)
				.SetCreate_time(item.Create_time)
				.SetTitle(item.Title).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected SysdocInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(SysdocInfo item, uint? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 cnodejs.DAL.Sysdoc.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("cnodejs.DAL.Sysdoc.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetContent(string value) {
				if (_item != null) _item.Content = value;
				return this.Set("`content`", string.Concat("?content_", _parameters.Count), 
					GetParameter(string.Concat("?content_", _parameters.Count), MySqlDbType.Text, -1, value));
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", string.Concat("?create_time_", _parameters.Count), 
					GetParameter(string.Concat("?create_time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetTitle(string value) {
				if (_item != null) _item.Title = value;
				return this.Set("`title`", string.Concat("?title_", _parameters.Count), 
					GetParameter(string.Concat("?title_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
		}
		#endregion

		public SysdocInfo Insert(SysdocInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public SysdocInfo GetItem(uint? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
	}
}