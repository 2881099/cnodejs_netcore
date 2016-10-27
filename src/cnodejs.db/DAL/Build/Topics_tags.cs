using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.DAL {

	public partial class Topics_tags : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`topics_tags`";
			internal static readonly string Field = "a.`tags_id`, a.`topics_id`";
			internal static readonly string Sort = "a.`tags_id`, a.`topics_id`";
			public static readonly string Delete = "DELETE FROM `topics_tags` WHERE ";
			public static readonly string Insert = "INSERT INTO `topics_tags`(`tags_id`, `topics_id`) VALUES(?tags_id, ?topics_id)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(Topics_tagsInfo item) {
			return new MySqlParameter[] {
				GetParameter("?tags_id", MySqlDbType.UInt32, 10, item.Tags_id), 
				GetParameter("?topics_id", MySqlDbType.UInt64, 20, item.Topics_id)};
		}
		public Topics_tagsInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as Topics_tagsInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new Topics_tagsInfo {
				Tags_id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Topics_id = dr.IsDBNull(++index) ? null : (ulong?)dr.GetInt64(index)};
		}
		public SelectBuild<Topics_tagsInfo> Select {
			get { return SelectBuild<Topics_tagsInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Tags_id, ulong? Topics_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`tags_id` = ?tags_id AND `topics_id` = ?topics_id"), 
				GetParameter("?tags_id", MySqlDbType.UInt32, 10, Tags_id), 
				GetParameter("?topics_id", MySqlDbType.UInt64, 20, Topics_id));
		}
		public int DeleteByTopics_id(ulong? Topics_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`topics_id` = ?topics_id"), 
				GetParameter("?topics_id", MySqlDbType.UInt64, 20, Topics_id));
		}
		public int DeleteByTags_id(uint? Tags_id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`tags_id` = ?tags_id"), 
				GetParameter("?tags_id", MySqlDbType.UInt32, 10, Tags_id));
		}

		public int Update(Topics_tagsInfo item) {
			return new SqlUpdateBuild(null, item.Tags_id, item.Topics_id).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected Topics_tagsInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(Topics_tagsInfo item, uint? Tags_id, ulong? Topics_id) {
				_item = item;
				_where = SqlHelper.Addslashes("`tags_id` = {0} AND `topics_id` = {1}", Tags_id, Topics_id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 cnodejs.DAL.Topics_tags.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("cnodejs.DAL.Topics_tags.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
		}
		#endregion

		public Topics_tagsInfo Insert(Topics_tagsInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public Topics_tagsInfo GetItem(uint? Tags_id, ulong? Topics_id) {
			return this.Select.Where("a.`tags_id` = {0} AND a.`topics_id` = {1}", Tags_id, Topics_id).ToOne();
		}
	}
}