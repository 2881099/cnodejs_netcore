using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.BLL {

	public partial class Topics {

		protected static readonly cnodejs.DAL.Topics dal = new cnodejs.DAL.Topics();
		protected static readonly int itemCacheTimeout;

		static Topics() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT_Topics"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(ulong? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByLast_posts_id(ulong? Last_posts_id) {
			return dal.DeleteByLast_posts_id(Last_posts_id);
		}
		public static int DeleteByOwner_users_id(ulong? Owner_users_id) {
			return dal.DeleteByOwner_users_id(Owner_users_id);
		}

		public static int Update(TopicsInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static cnodejs.DAL.Topics.SqlUpdateBuild UpdateDiy(ulong? Id) {
			return UpdateDiy(null, Id);
		}
		public static cnodejs.DAL.Topics.SqlUpdateBuild UpdateDiy(TopicsInfo item, ulong? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new cnodejs.DAL.Topics.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static cnodejs.DAL.Topics.SqlUpdateBuild UpdateDiyDangerous {
			get { return new cnodejs.DAL.Topics.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Topics.Insert(TopicsInfo item)
		/// </summary>
		[Obsolete]
		public static TopicsInfo Insert(ulong? Last_posts_id, ulong? Owner_users_id, int? Count_posts, uint? Count_views, DateTime? Create_time, string Title, ulong? Top, DateTime? Update_time) {
			return Insert(new TopicsInfo {
				Last_posts_id = Last_posts_id, 
				Owner_users_id = Owner_users_id, 
				Count_posts = Count_posts, 
				Count_views = Count_views, 
				Create_time = Create_time, 
				Title = Title, 
				Top = Top, 
				Update_time = Update_time});
		}
		public static TopicsInfo Insert(TopicsInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(TopicsInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("cnodejs_BLL_Topics_", item.Id));
		}
		#endregion

		public static TopicsInfo GetItem(ulong? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("cnodejs_BLL_Topics_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new TopicsInfo(value); } catch { }
			TopicsInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<TopicsInfo> GetItems() {
			return Select.ToList();
		}
		public static TopicsSelectBuild Select {
			get { return new TopicsSelectBuild(dal); }
		}
		public static List<TopicsInfo> GetItemsByLast_posts_id(params ulong?[] Last_posts_id) {
			return Select.WhereLast_posts_id(Last_posts_id).ToList();
		}
		public static List<TopicsInfo> GetItemsByLast_posts_id(ulong?[] Last_posts_id, int limit) {
			return Select.WhereLast_posts_id(Last_posts_id).Limit(limit).ToList();
		}
		public static TopicsSelectBuild SelectByLast_posts_id(params ulong?[] Last_posts_id) {
			return Select.WhereLast_posts_id(Last_posts_id);
		}
		public static List<TopicsInfo> GetItemsByOwner_users_id(params ulong?[] Owner_users_id) {
			return Select.WhereOwner_users_id(Owner_users_id).ToList();
		}
		public static List<TopicsInfo> GetItemsByOwner_users_id(ulong?[] Owner_users_id, int limit) {
			return Select.WhereOwner_users_id(Owner_users_id).Limit(limit).ToList();
		}
		public static TopicsSelectBuild SelectByOwner_users_id(params ulong?[] Owner_users_id) {
			return Select.WhereOwner_users_id(Owner_users_id);
		}
		public static TopicsSelectBuild SelectByTags(params TagsInfo[] items) {
			return Select.WhereTags(items);
		}
		public static TopicsSelectBuild SelectByTags_id(params uint[] ids) {
			return Select.WhereTags_id(ids);
		}
		public static TopicsSelectBuild SelectByUsers(params UsersInfo[] items) {
			return Select.WhereUsers(items);
		}
		public static TopicsSelectBuild SelectByUsers_id(params ulong[] ids) {
			return Select.WhereUsers_id(ids);
		}
	}
	public partial class TopicsSelectBuild : SelectBuild<TopicsInfo, TopicsSelectBuild> {
		public TopicsSelectBuild WhereLast_posts_id(params ulong?[] Last_posts_id) {
			return this.Where1Or("a.`Last_posts_id` = {0}", Last_posts_id);
		}
		public TopicsSelectBuild WhereOwner_users_id(params ulong?[] Owner_users_id) {
			return this.Where1Or("a.`Owner_users_id` = {0}", Owner_users_id);
		}
		public TopicsSelectBuild WhereTags(params TagsInfo[] items) {
			if (items == null) return this;
			return WhereTags_id(items.Where<TagsInfo>(a => a != null).Select<TagsInfo, uint>(a => a.Id.Value).ToArray());
		}
		public TopicsSelectBuild WhereTags_id(params uint[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `topics_id` FROM `topics_tags` WHERE `topics_id` = a.`id` AND `tags_id` IN ({0}) )", string.Join<uint>(",", ids))) as TopicsSelectBuild;
		}
		public TopicsSelectBuild WhereUsers(params UsersInfo[] items) {
			if (items == null) return this;
			return WhereUsers_id(items.Where<UsersInfo>(a => a != null).Select<UsersInfo, ulong>(a => a.Id.Value).ToArray());
		}
		public TopicsSelectBuild WhereUsers_id(params ulong[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `topics_id` FROM `users_topics` WHERE `topics_id` = a.`id` AND `users_id` IN ({0}) )", string.Join<ulong>(",", ids))) as TopicsSelectBuild;
		}
		public TopicsSelectBuild WhereId(params ulong?[] Id) {
			return this.Where1Or("a.`Id` = {0}", Id);
		}
		public TopicsSelectBuild WhereCount_posts(params int?[] Count_posts) {
			return this.Where1Or("a.`Count_posts` = {0}", Count_posts);
		}
		public TopicsSelectBuild WhereCount_views(params uint?[] Count_views) {
			return this.Where1Or("a.`Count_views` = {0}", Count_views);
		}
		public TopicsSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`Create_time` >= {0}", begin) as TopicsSelectBuild;
		}
		public TopicsSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`Create_time` between {0} and {1}", begin, end) as TopicsSelectBuild;
		}
		public TopicsSelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`Title` = {0}", Title);
		}
		public TopicsSelectBuild WhereTop(params ulong?[] Top) {
			return this.Where1Or("a.`Top` = {0}", Top);
		}
		public TopicsSelectBuild WhereUpdate_timeRange(DateTime? begin) {
			return base.Where("a.`Update_time` >= {0}", begin) as TopicsSelectBuild;
		}
		public TopicsSelectBuild WhereUpdate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereUpdate_timeRange(begin);
			return base.Where("a.`Update_time` between {0} and {1}", begin, end) as TopicsSelectBuild;
		}
		protected new TopicsSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as TopicsSelectBuild;
		}
		public TopicsSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}