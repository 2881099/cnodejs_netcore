using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.DAL {

	public partial class Users : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`users`";
			internal static readonly string Field = "a.`id`, a.`create_time`, a.`email`, a.`github`, a.`location`, a.`password`, a.`point`, a.`sign`, a.`username`, a.`website`, a.`weibo`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `users` WHERE ";
			public static readonly string Insert = "INSERT INTO `users`(`create_time`, `email`, `github`, `location`, `password`, `point`, `sign`, `username`, `website`, `weibo`) VALUES(?create_time, ?email, ?github, ?location, ?password, ?point, ?sign, ?username, ?website, ?weibo); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(UsersInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt64, 20, item.Id), 
				GetParameter("?create_time", MySqlDbType.DateTime, -1, item.Create_time), 
				GetParameter("?email", MySqlDbType.VarChar, 64, item.Email), 
				GetParameter("?github", MySqlDbType.VarChar, 96, item.Github), 
				GetParameter("?location", MySqlDbType.VarChar, 64, item.Location), 
				GetParameter("?password", MySqlDbType.VarChar, 32, item.Password), 
				GetParameter("?point", MySqlDbType.UInt32, 10, item.Point), 
				GetParameter("?sign", MySqlDbType.VarChar, 255, item.Sign), 
				GetParameter("?username", MySqlDbType.VarChar, 64, item.Username), 
				GetParameter("?website", MySqlDbType.VarChar, 96, item.Website), 
				GetParameter("?weibo", MySqlDbType.VarChar, 96, item.Weibo)};
		}
		public UsersInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as UsersInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new UsersInfo {
				Id = dr.IsDBNull(++index) ? null : (ulong?)dr.GetInt64(index), 
				Create_time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Email = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Github = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Location = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Password = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Point = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Sign = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Username = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Website = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Weibo = dr.IsDBNull(++index) ? null : (string)dr.GetString(index)};
		}
		public SelectBuild<UsersInfo> Select {
			get { return SelectBuild<UsersInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(ulong? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt64, 20, Id));
		}
		public int DeleteByUsername(string Username) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`username` = ?username"), 
				GetParameter("?username", MySqlDbType.VarChar, 64, Username));
		}
		public int DeleteByEmail(string Email) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`email` = ?email"), 
				GetParameter("?email", MySqlDbType.VarChar, 64, Email));
		}

		public int Update(UsersInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetCreate_time(item.Create_time)
				.SetEmail(item.Email)
				.SetGithub(item.Github)
				.SetLocation(item.Location)
				.SetPassword(item.Password)
				.SetPoint(item.Point)
				.SetSign(item.Sign)
				.SetUsername(item.Username)
				.SetWebsite(item.Website)
				.SetWeibo(item.Weibo).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected UsersInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(UsersInfo item, ulong? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 cnodejs.DAL.Users.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("cnodejs.DAL.Users.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetCreate_time(DateTime? value) {
				if (_item != null) _item.Create_time = value;
				return this.Set("`create_time`", string.Concat("?create_time_", _parameters.Count), 
					GetParameter(string.Concat("?create_time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetEmail(string value) {
				if (_item != null) _item.Email = value;
				return this.Set("`email`", string.Concat("?email_", _parameters.Count), 
					GetParameter(string.Concat("?email_", _parameters.Count), MySqlDbType.VarChar, 64, value));
			}
			public SqlUpdateBuild SetGithub(string value) {
				if (_item != null) _item.Github = value;
				return this.Set("`github`", string.Concat("?github_", _parameters.Count), 
					GetParameter(string.Concat("?github_", _parameters.Count), MySqlDbType.VarChar, 96, value));
			}
			public SqlUpdateBuild SetLocation(string value) {
				if (_item != null) _item.Location = value;
				return this.Set("`location`", string.Concat("?location_", _parameters.Count), 
					GetParameter(string.Concat("?location_", _parameters.Count), MySqlDbType.VarChar, 64, value));
			}
			public SqlUpdateBuild SetPassword(string value) {
				if (_item != null) _item.Password = value;
				return this.Set("`password`", string.Concat("?password_", _parameters.Count), 
					GetParameter(string.Concat("?password_", _parameters.Count), MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetPoint(uint? value) {
				if (_item != null) _item.Point = value;
				return this.Set("`point`", string.Concat("?point_", _parameters.Count), 
					GetParameter(string.Concat("?point_", _parameters.Count), MySqlDbType.UInt32, 10, value));
			}
			public SqlUpdateBuild SetPointIncrement(int value) {
				if (_item != null) _item.Point = (uint?)((int?)_item.Point + value);
				return this.Set("`point`", string.Concat("`point` + ?point_", _parameters.Count), 
					GetParameter(string.Concat("?point_", _parameters.Count), MySqlDbType.Int32, 10, value));
			}
			public SqlUpdateBuild SetSign(string value) {
				if (_item != null) _item.Sign = value;
				return this.Set("`sign`", string.Concat("?sign_", _parameters.Count), 
					GetParameter(string.Concat("?sign_", _parameters.Count), MySqlDbType.VarChar, 255, value));
			}
			public SqlUpdateBuild SetUsername(string value) {
				if (_item != null) _item.Username = value;
				return this.Set("`username`", string.Concat("?username_", _parameters.Count), 
					GetParameter(string.Concat("?username_", _parameters.Count), MySqlDbType.VarChar, 64, value));
			}
			public SqlUpdateBuild SetWebsite(string value) {
				if (_item != null) _item.Website = value;
				return this.Set("`website`", string.Concat("?website_", _parameters.Count), 
					GetParameter(string.Concat("?website_", _parameters.Count), MySqlDbType.VarChar, 96, value));
			}
			public SqlUpdateBuild SetWeibo(string value) {
				if (_item != null) _item.Weibo = value;
				return this.Set("`weibo`", string.Concat("?weibo_", _parameters.Count), 
					GetParameter(string.Concat("?weibo_", _parameters.Count), MySqlDbType.VarChar, 96, value));
			}
		}
		#endregion

		public UsersInfo Insert(UsersInfo item) {
			ulong loc1;
			if (ulong.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public UsersInfo GetItem(ulong? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
		public UsersInfo GetItemByUsername(string Username) {
			return this.Select.Where("a.`username` = {0}", Username).ToOne();
		}
		public UsersInfo GetItemByEmail(string Email) {
			return this.Select.Where("a.`email` = {0}", Email).ToOne();
		}
	}
}