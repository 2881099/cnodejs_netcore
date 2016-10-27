using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.DAL {

	public partial class Posts : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`posts`";
			internal static readonly string Field = "a.`id`, a.`posts_id`, a.`topics_id`, a.`users_id`, a.`content`, a.`count_good`, a.`count_notgood`, a.`create_time`, a.`index`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `posts` WHERE ";
			public static readonly string Insert = "INSERT INTO `posts`(`posts_id`, `topics_id`, `users_id`, `content`, `count_good`, `count_notgood`, `create_time`, `index`) VALUES(?posts_id, ?topics_id, ?users_id, ?content, ?count_good, ?count_notgood, ?create_time, ?index); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(PostsInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt64, 20, item.Id), 
				GetParameter("?posts_id", MySqlDbType.UInt64, 20, item.Posts_id), 
				GetParameter("?topics_id", MySqlDbType.UInt64, 20, item.Topics_id), 
				GetParameter("?users_id", MySqlDbType.UInt64, 20, item.Users_id), 
				GetParameter("?content", MySqlDbType.Text, -1, item.Content), 
				GetParameter("?count_good", MySqlDbType.Int32, 11, item.Count_good), 
				GetParameter("?count_notgood", MySqlDbType.Int32, 11, item.Count_notgood), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?index", MySqlDbType.UInt32, 10, item.Index)};
		}
		public PostsInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as PostsInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new PostsInfo {
				Id = dr.IsDBNull(++index) ? null : (ulong?)dr.GetInt64(index), 
				Posts_id = dr.IsDBNull(++index) ? null : (ulong?)dr.GetInt64(index), 
				Topics_id = dr.IsDBNull(++index) ? null : (ulong?)dr.GetInt64(index), 
				Users_id = dr.IsDBNull(++index) ? null : (ulong?)dr.GetInt64(index), 
				Content = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Count_good = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				Count_notgood = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				Create_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Index = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index)};
		}
		public SelectBuild<PostsInfo> Select {
			get { return SelectBuild<PostsInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(ulong? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt64, 20, Id));
		}
		public int DeleteByIndexAndTopics_id(uint? Index, ulong? Topics_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`index` = ?index AND `topics_id` = ?topics_id"), 
				GetParameter("?index", MySqlDbType.UInt32, 10, Index), 
				GetParameter("?topics_id", MySqlDbType.UInt64, 20, Topics_id));
		}
		public int DeleteByPosts_id(ulong? Posts_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`posts_id` = ?posts_id"), 
				GetParameter("?posts_id", MySqlDbType.UInt64, 20, Posts_id));
		}
		public int DeleteByTopics_id(ulong? Topics_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`topics_id` = ?topics_id"), 
				GetParameter("?topics_id", MySqlDbType.UInt64, 20, Topics_id));
		}
		public int DeleteByUsers_id(ulong? Users_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`users_id` = ?users_id"), 
				GetParameter("?users_id", MySqlDbType.UInt64, 20, Users_id));
		}

		public int Update(PostsInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetPosts_id(item.Posts_id)
				.SetTopics_id(item.Topics_id)
				.SetUsers_id(item.Users_id)
				.SetContent(item.Content)
				.SetCount_good(item.Count_good)
				.SetCount_notgood(item.Count_notgood)
				.SetCreate_time(item.Create_time)
				.SetIndex(item.Index).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected PostsInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(PostsInfo item, ulong? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 cnodejs.DAL.Posts.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("cnodejs.DAL.Posts.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetPosts_id(ulong? value) {
				if (_item != null) _item.Posts_id = value;
				return this.Set("`posts_id`", string.Concat("?posts_id_", _parameters.Count), 
					GetParameter(string.Concat("?posts_id_", _parameters.Count), MySqlDbType.UInt64, 20, value));
			}
			public SqlUpdateBuild SetTopics_id(ulong? value) {
				if (_item != null) _item.Topics_id = value;
				return this.Set("`topics_id`", string.Concat("?topics_id_", _parameters.Count), 
					GetParameter(string.Concat("?topics_id_", _parameters.Count), MySqlDbType.UInt64, 20, value));
			}
			public SqlUpdateBuild SetUsers_id(ulong? value) {
				if (_item != null) _item.Users_id = value;
				return this.Set("`users_id`", string.Concat("?users_id_", _parameters.Count), 
					GetParameter(string.Concat("?users_id_", _parameters.Count), MySqlDbType.UInt64, 20, value));
			}
			public SqlUpdateBuild SetContent(string value) {
				if (_item != null) _item.Content = value;
				return this.Set("`content`", string.Concat("?content_", _parameters.Count), 
					GetParameter(string.Concat("?content_", _parameters.Count), MySqlDbType.Text, -1, value));
			}
			public SqlUpdateBuild SetCount_good(int? value) {
				if (_item != null) _item.Count_good = value;
				return this.Set("`count_good`", string.Concat("?count_good_", _parameters.Count), 
					GetParameter(string.Concat("?count_good_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetCount_goodIncrement(int value) {
				if (_item != null) _item.Count_good += value;
				return this.Set("`count_good`", string.Concat("`count_good` + ?count_good_", _parameters.Count), 
					GetParameter(string.Concat("?count_good_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetCount_notgood(int? value) {
				if (_item != null) _item.Count_notgood = value;
				return this.Set("`count_notgood`", string.Concat("?count_notgood_", _parameters.Count), 
					GetParameter(string.Concat("?count_notgood_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetCount_notgoodIncrement(int value) {
				if (_item != null) _item.Count_notgood += value;
				return this.Set("`count_notgood`", string.Concat("`count_notgood` + ?count_notgood_", _parameters.Count), 
					GetParameter(string.Concat("?count_notgood_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", string.Concat("?create_time_", _parameters.Count), 
					GetParameter(string.Concat("?create_time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetIndex(uint? value) {
				if (_item != null) _item.Index = value;
				return this.Set("`index`", string.Concat("?index_", _parameters.Count), 
					GetParameter(string.Concat("?index_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetIndexIncrement(int value) {
				if (_item != null) _item.Index = (uint?)((int?)_item.Index + value);
				return this.Set("`index`", string.Concat("`index` + ?index_", _parameters.Count), 
					GetParameter(string.Concat("?index_", _parameters.Count), MySqlDbType.Int32, 10, value));
			}
		}
		#endregion

		public PostsInfo Insert(PostsInfo item) {
			ulong loc1;
			if (ulong.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public PostsInfo GetItem(ulong? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
		public PostsInfo GetItemByIndexAndTopics_id(uint? Index, ulong? Topics_id) {
			return this.Select.Where("a.`index` = {0} AND a.`topics_id` = {1}", Index, Topics_id).ToOne();
		}
	}
}