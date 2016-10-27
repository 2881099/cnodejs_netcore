using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.BLL {

	public partial class Users_topics {

		protected static readonly cnodejs.DAL.Users_topics dal = new cnodejs.DAL.Users_topics();
		protected static readonly int itemCacheTimeout;

		static Users_topics() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT_Users_topics"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(ulong? Topics_id, ulong? Users_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Topics_id, Users_id));
			return dal.Delete(Topics_id, Users_id);
		}
		public static int DeleteByTopics_id(ulong? Topics_id) {
			return dal.DeleteByTopics_id(Topics_id);
		}
		public static int DeleteByUsers_id(ulong? Users_id) {
			return dal.DeleteByUsers_id(Users_id);
		}

		public static int Update(Users_topicsInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static cnodejs.DAL.Users_topics.SqlUpdateBuild UpdateDiy(ulong? Topics_id, ulong? Users_id) {
			return UpdateDiy(null, Topics_id, Users_id);
		}
		public static cnodejs.DAL.Users_topics.SqlUpdateBuild UpdateDiy(Users_topicsInfo item, ulong? Topics_id, ulong? Users_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Topics_id, Users_id));
			return new cnodejs.DAL.Users_topics.SqlUpdateBuild(item, Topics_id, Users_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static cnodejs.DAL.Users_topics.SqlUpdateBuild UpdateDiyDangerous {
			get { return new cnodejs.DAL.Users_topics.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Users_topics.Insert(Users_topicsInfo item)
		/// </summary>
		[Obsolete]
		public static Users_topicsInfo Insert(ulong? Topics_id, ulong? Users_id) {
			return Insert(new Users_topicsInfo {
				Topics_id = Topics_id, 
				Users_id = Users_id});
		}
		public static Users_topicsInfo Insert(Users_topicsInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Users_topicsInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("cnodejs_BLL_Users_topics_", item.Topics_id, "_,_", item.Users_id));
		}
		#endregion

		public static Users_topicsInfo GetItem(ulong? Topics_id, ulong? Users_id) {
			if (Topics_id == null || Users_id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Topics_id, Users_id);
			string key = string.Concat("cnodejs_BLL_Users_topics_", Topics_id, "_,_", Users_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new Users_topicsInfo(value); } catch { }
			Users_topicsInfo item = dal.GetItem(Topics_id, Users_id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Users_topicsInfo> GetItems() {
			return Select.ToList();
		}
		public static Users_topicsSelectBuild Select {
			get { return new Users_topicsSelectBuild(dal); }
		}
		public static List<Users_topicsInfo> GetItemsByTopics_id(params ulong?[] Topics_id) {
			return Select.WhereTopics_id(Topics_id).ToList();
		}
		public static List<Users_topicsInfo> GetItemsByTopics_id(ulong?[] Topics_id, int limit) {
			return Select.WhereTopics_id(Topics_id).Limit(limit).ToList();
		}
		public static Users_topicsSelectBuild SelectByTopics_id(params ulong?[] Topics_id) {
			return Select.WhereTopics_id(Topics_id);
		}
		public static List<Users_topicsInfo> GetItemsByUsers_id(params ulong?[] Users_id) {
			return Select.WhereUsers_id(Users_id).ToList();
		}
		public static List<Users_topicsInfo> GetItemsByUsers_id(ulong?[] Users_id, int limit) {
			return Select.WhereUsers_id(Users_id).Limit(limit).ToList();
		}
		public static Users_topicsSelectBuild SelectByUsers_id(params ulong?[] Users_id) {
			return Select.WhereUsers_id(Users_id);
		}
	}
	public partial class Users_topicsSelectBuild : SelectBuild<Users_topicsInfo, Users_topicsSelectBuild> {
		public Users_topicsSelectBuild WhereTopics_id(params ulong?[] Topics_id) {
			return this.Where1Or("a.`Topics_id` = {0}", Topics_id);
		}
		public Users_topicsSelectBuild WhereUsers_id(params ulong?[] Users_id) {
			return this.Where1Or("a.`Users_id` = {0}", Users_id);
		}
		protected new Users_topicsSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Users_topicsSelectBuild;
		}
		public Users_topicsSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}