using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.DAL {

	public partial class Friendlylinks : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`friendlylinks`";
			internal static readonly string Field = "a.`id`, a.`create_time`, a.`link`, a.`logo`, a.`sort`, a.`title`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `friendlylinks` WHERE ";
			public static readonly string Insert = "INSERT INTO `friendlylinks`(`create_time`, `link`, `logo`, `sort`, `title`) VALUES(?create_time, ?link, ?logo, ?sort, ?title); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(FriendlylinksInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?link", MySqlDbType.VarChar, 255, item.Link), 
				GetParameter("?logo", MySqlDbType.VarChar, 96, item.Logo), 
				GetParameter("?sort", MySqlDbType.UInt32, 10, item.Sort), 
				GetParameter("?title", MySqlDbType.VarChar, 96, item.Title)};
		}
		public FriendlylinksInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as FriendlylinksInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new FriendlylinksInfo {
				Id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Create_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Link = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Logo = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Sort = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Title = dr.IsDBNull(++index) ? null : (string)dr.GetString(index)};
		}
		public SelectBuild<FriendlylinksInfo> Select {
			get { return SelectBuild<FriendlylinksInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}

		public int Update(FriendlylinksInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetCreate_time(item.Create_time)
				.SetLink(item.Link)
				.SetLogo(item.Logo)
				.SetSort(item.Sort)
				.SetTitle(item.Title).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected FriendlylinksInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(FriendlylinksInfo item, uint? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 cnodejs.DAL.Friendlylinks.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("cnodejs.DAL.Friendlylinks.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", string.Concat("?create_time_", _parameters.Count), 
					GetParameter(string.Concat("?create_time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetLink(string value) {
				if (_item != null) _item.Link = value;
				return this.Set("`link`", string.Concat("?link_", _parameters.Count), 
					GetParameter(string.Concat("?link_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetLogo(string value) {
				if (_item != null) _item.Logo = value;
				return this.Set("`logo`", string.Concat("?logo_", _parameters.Count), 
					GetParameter(string.Concat("?logo_", _parameters.Count), MySqlDbType.VarChar, 96, value));
			}
			public SqlUpdateBuild SetSort(uint? value) {
				if (_item != null) _item.Sort = value;
				return this.Set("`sort`", string.Concat("?sort_", _parameters.Count), 
					GetParameter(string.Concat("?sort_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetSortIncrement(int value) {
				if (_item != null) _item.Sort = (uint?)((int?)_item.Sort + value);
				return this.Set("`sort`", string.Concat("`sort` + ?sort_", _parameters.Count), 
					GetParameter(string.Concat("?sort_", _parameters.Count), MySqlDbType.Int32, 10, value));
			}
			public SqlUpdateBuild SetTitle(string value) {
				if (_item != null) _item.Title = value;
				return this.Set("`title`", string.Concat("?title_", _parameters.Count), 
					GetParameter(string.Concat("?title_", _parameters.Count), MySqlDbType.VarChar, 96, value));
			}
		}
		#endregion

		public FriendlylinksInfo Insert(FriendlylinksInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public FriendlylinksInfo GetItem(uint? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
	}
}