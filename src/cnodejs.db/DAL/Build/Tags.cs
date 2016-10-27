using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.DAL {

	public partial class Tags : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`tags`";
			internal static readonly string Field = "a.`id`, a.`create_time`, a.`keyname`, a.`name`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `tags` WHERE ";
			public static readonly string Insert = "INSERT INTO `tags`(`create_time`, `keyname`, `name`) VALUES(?create_time, ?keyname, ?name); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(TagsInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?keyname", MySqlDbType.VarChar, 32, item.Keyname), 
				GetParameter("?name", MySqlDbType.VarChar, 96, item.Name)};
		}
		public TagsInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as TagsInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new TagsInfo {
				Id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Create_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Keyname = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Name = dr.IsDBNull(++index) ? null : (string)dr.GetString(index)};
		}
		public SelectBuild<TagsInfo> Select {
			get { return SelectBuild<TagsInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByKeyname(string Keyname) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`keyname` = ?keyname"), 
				GetParameter("?keyname", MySqlDbType.VarChar, 32, Keyname));
		}

		public int Update(TagsInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetCreate_time(item.Create_time)
				.SetKeyname(item.Keyname)
				.SetName(item.Name).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected TagsInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(TagsInfo item, uint? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 cnodejs.DAL.Tags.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("cnodejs.DAL.Tags.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", string.Concat("?create_time_", _parameters.Count), 
					GetParameter(string.Concat("?create_time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetKeyname(string value) {
				if (_item != null) _item.Keyname = value;
				return this.Set("`keyname`", string.Concat("?keyname_", _parameters.Count), 
					GetParameter(string.Concat("?keyname_", _parameters.Count), MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetName(string value) {
				if (_item != null) _item.Name = value;
				return this.Set("`name`", string.Concat("?name_", _parameters.Count), 
					GetParameter(string.Concat("?name_", _parameters.Count), MySqlDbType.VarChar, 96, value));
			}
		}
		#endregion

		public TagsInfo Insert(TagsInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public TagsInfo GetItem(uint? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
		public TagsInfo GetItemByKeyname(string Keyname) {
			return this.Select.Where("a.`keyname` = {0}", Keyname).ToOne();
		}
	}
}