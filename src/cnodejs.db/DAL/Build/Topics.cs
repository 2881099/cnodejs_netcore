using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.DAL {

	public partial class Topics : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`topics`";
			internal static readonly string Field = "a.`id`, a.`last_posts_id`, a.`owner_users_id`, a.`count_posts`, a.`count_views`, a.`create_time`, a.`title`, a.`top`, a.`update_time`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `topics` WHERE ";
			public static readonly string Insert = "INSERT INTO `topics`(`last_posts_id`, `owner_users_id`, `count_posts`, `count_views`, `create_time`, `title`, `top`, `update_time`) VALUES(?last_posts_id, ?owner_users_id, ?count_posts, ?count_views, ?create_time, ?title, ?top, ?update_time); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(TopicsInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt64, 20, item.Id), 
				GetParameter("?last_posts_id", MySqlDbType.UInt64, 20, item.Last_posts_id), 
				GetParameter("?owner_users_id", MySqlDbType.UInt64, 20, item.Owner_users_id), 
				GetParameter("?count_posts", MySqlDbType.Int32, 11, item.Count_posts), 
				GetParameter("?count_views", MySqlDbType.UInt32, 10, item.Count_views), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?title", MySqlDbType.VarChar, 255, item.Title), 
				GetParameter("?top", MySqlDbType.UInt64, 20, item.Top), 
				GetParameter("?update_time", MySqlDbType.DateTime, -1, item.Update_time)};
		}
		public TopicsInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as TopicsInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new TopicsInfo {
				Id = dr.IsDBNull(++index) ? null : (ulong?)dr.GetInt64(index), 
				Last_posts_id = dr.IsDBNull(++index) ? null : (ulong?)dr.GetInt64(index), 
				Owner_users_id = dr.IsDBNull(++index) ? null : (ulong?)dr.GetInt64(index), 
				Count_posts = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				Count_views = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Create_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Title = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Top = dr.IsDBNull(++index) ? null : (ulong?)dr.GetInt64(index), 
				Update_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index)};
		}
		public SelectBuild<TopicsInfo> Select {
			get { return SelectBuild<TopicsInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(ulong? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt64, 20, Id));
		}
		public int DeleteByLast_posts_id(ulong? Last_posts_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`last_posts_id` = ?last_posts_id"), 
				GetParameter("?last_posts_id", MySqlDbType.UInt64, 20, Last_posts_id));
		}
		public int DeleteByOwner_users_id(ulong? Owner_users_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`owner_users_id` = ?owner_users_id"), 
				GetParameter("?owner_users_id", MySqlDbType.UInt64, 20, Owner_users_id));
		}

		public int Update(TopicsInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetLast_posts_id(item.Last_posts_id)
				.SetOwner_users_id(item.Owner_users_id)
				.SetCount_posts(item.Count_posts)
				.SetCount_views(item.Count_views)
				.SetCreate_time(item.Create_time)
				.SetTitle(item.Title)
				.SetTop(item.Top)
				.SetUpdate_time(item.Update_time).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected TopicsInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(TopicsInfo item, ulong? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 cnodejs.DAL.Topics.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("cnodejs.DAL.Topics.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetLast_posts_id(ulong? value) {
				if (_item != null) _item.Last_posts_id = value;
				return this.Set("`last_posts_id`", string.Concat("?last_posts_id_", _parameters.Count), 
					GetParameter(string.Concat("?last_posts_id_", _parameters.Count), MySqlDbType.UInt64, 20, value));
			}
			public SqlUpdateBuild SetOwner_users_id(ulong? value) {
				if (_item != null) _item.Owner_users_id = value;
				return this.Set("`owner_users_id`", string.Concat("?owner_users_id_", _parameters.Count), 
					GetParameter(string.Concat("?owner_users_id_", _parameters.Count), MySqlDbType.UInt64, 20, value));
			}
			public SqlUpdateBuild SetCount_posts(int? value) {
				if (_item != null) _item.Count_posts = value;
				return this.Set("`count_posts`", string.Concat("?count_posts_", _parameters.Count), 
					GetParameter(string.Concat("?count_posts_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetCount_postsIncrement(int value) {
				if (_item != null) _item.Count_posts += value;
				return this.Set("`count_posts`", string.Concat("`count_posts` + ?count_posts_", _parameters.Count), 
					GetParameter(string.Concat("?count_posts_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetCount_views(uint? value) {
				if (_item != null) _item.Count_views = value;
				return this.Set("`count_views`", string.Concat("?count_views_", _parameters.Count), 
					GetParameter(string.Concat("?count_views_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetCount_viewsIncrement(int value) {
				if (_item != null) _item.Count_views = (uint?)((int?)_item.Count_views + value);
				return this.Set("`count_views`", string.Concat("`count_views` + ?count_views_", _parameters.Count), 
					GetParameter(string.Concat("?count_views_", _parameters.Count), MySqlDbType.Int32, 10, value));
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
			public SqlUpdateBuild SetTop(ulong? value) {
				if (_item != null) _item.Top = value;
				return this.Set("`top`", string.Concat("?top_", _parameters.Count), 
					GetParameter(string.Concat("?top_", _parameters.Count), MySqlDbType.UInt64, 20, value));
			}
			public SqlUpdateBuild SetTopIncrement(long value) {
				if (_item != null) _item.Top = (ulong?)((long?)_item.Top + value);
				return this.Set("`top`", string.Concat("`top` + ?top_", _parameters.Count), 
					GetParameter(string.Concat("?top_", _parameters.Count), MySqlDbType.Int64, 20, value));
			}
			public SqlUpdateBuild SetUpdate_time(DateTime? value) {
				if (_item != null) _item.Update_time = value;
				return this.Set("`update_time`", string.Concat("?update_time_", _parameters.Count), 
					GetParameter(string.Concat("?update_time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
		}
		#endregion

		public TopicsInfo Insert(TopicsInfo item) {
			ulong loc1;
			if (ulong.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public TopicsInfo GetItem(ulong? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
	}
}