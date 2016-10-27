using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.BLL {

	public partial class Posts {

		protected static readonly cnodejs.DAL.Posts dal = new cnodejs.DAL.Posts();
		protected static readonly int itemCacheTimeout;

		static Posts() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT_Posts"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(ulong? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByIndexAndTopics_id(uint? Index, ulong? Topics_id) {
			return dal.DeleteByIndexAndTopics_id(Index, Topics_id);
		}
		public static int DeleteByPosts_id(ulong? Posts_id) {
			return dal.DeleteByPosts_id(Posts_id);
		}
		public static int DeleteByTopics_id(ulong? Topics_id) {
			return dal.DeleteByTopics_id(Topics_id);
		}
		public static int DeleteByUsers_id(ulong? Users_id) {
			return dal.DeleteByUsers_id(Users_id);
		}

		public static int Update(PostsInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static cnodejs.DAL.Posts.SqlUpdateBuild UpdateDiy(ulong? Id) {
			return UpdateDiy(null, Id);
		}
		public static cnodejs.DAL.Posts.SqlUpdateBuild UpdateDiy(PostsInfo item, ulong? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new cnodejs.DAL.Posts.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static cnodejs.DAL.Posts.SqlUpdateBuild UpdateDiyDangerous {
			get { return new cnodejs.DAL.Posts.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Posts.Insert(PostsInfo item)
		/// </summary>
		[Obsolete]
		public static PostsInfo Insert(ulong? Posts_id, ulong? Topics_id, ulong? Users_id, string Content, int? Count_good, int? Count_notgood, DateTime? Create_time, uint? Index) {
			return Insert(new PostsInfo {
				Posts_id = Posts_id, 
				Topics_id = Topics_id, 
				Users_id = Users_id, 
				Content = Content, 
				Count_good = Count_good, 
				Count_notgood = Count_notgood, 
				Create_time = Create_time, 
				Index = Index});
		}
		public static PostsInfo Insert(PostsInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(PostsInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("cnodejs_BLL_Posts_", item.Id));
			RedisHelper.Remove(string.Concat("cnodejs_BLL_PostsByIndexAndTopics_id_", item.Index, "_,_", item.Topics_id));
		}
		#endregion

		public static PostsInfo GetItem(ulong? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("cnodejs_BLL_Posts_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new PostsInfo(value); } catch { }
			PostsInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}
		public static PostsInfo GetItemByIndexAndTopics_id(uint? Index, ulong? Topics_id) {
			if (Index == null || Topics_id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItemByIndexAndTopics_id(Index, Topics_id);
			string key = string.Concat("cnodejs_BLL_PostsByIndexAndTopics_id_", Index, "_,_", Topics_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new PostsInfo(value); } catch { }
			PostsInfo item = dal.GetItemByIndexAndTopics_id(Index, Topics_id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<PostsInfo> GetItems() {
			return Select.ToList();
		}
		public static PostsSelectBuild Select {
			get { return new PostsSelectBuild(dal); }
		}
		public static List<PostsInfo> GetItemsByPosts_id(params ulong?[] Posts_id) {
			return Select.WherePosts_id(Posts_id).ToList();
		}
		public static List<PostsInfo> GetItemsByPosts_id(ulong?[] Posts_id, int limit) {
			return Select.WherePosts_id(Posts_id).Limit(limit).ToList();
		}
		public static PostsSelectBuild SelectByPosts_id(params ulong?[] Posts_id) {
			return Select.WherePosts_id(Posts_id);
		}
		public static List<PostsInfo> GetItemsByTopics_id(params ulong?[] Topics_id) {
			return Select.WhereTopics_id(Topics_id).ToList();
		}
		public static List<PostsInfo> GetItemsByTopics_id(ulong?[] Topics_id, int limit) {
			return Select.WhereTopics_id(Topics_id).Limit(limit).ToList();
		}
		public static PostsSelectBuild SelectByTopics_id(params ulong?[] Topics_id) {
			return Select.WhereTopics_id(Topics_id);
		}
		public static List<PostsInfo> GetItemsByUsers_id(params ulong?[] Users_id) {
			return Select.WhereUsers_id(Users_id).ToList();
		}
		public static List<PostsInfo> GetItemsByUsers_id(ulong?[] Users_id, int limit) {
			return Select.WhereUsers_id(Users_id).Limit(limit).ToList();
		}
		public static PostsSelectBuild SelectByUsers_id(params ulong?[] Users_id) {
			return Select.WhereUsers_id(Users_id);
		}
	}
	public partial class PostsSelectBuild : SelectBuild<PostsInfo, PostsSelectBuild> {
		public PostsSelectBuild WherePosts_id(params ulong?[] Posts_id) {
			return this.Where1Or("a.`Posts_id` = {0}", Posts_id);
		}
		public PostsSelectBuild WhereTopics_id(params ulong?[] Topics_id) {
			return this.Where1Or("a.`Topics_id` = {0}", Topics_id);
		}
		public PostsSelectBuild WhereUsers_id(params ulong?[] Users_id) {
			return this.Where1Or("a.`Users_id` = {0}", Users_id);
		}
		public PostsSelectBuild WhereId(params ulong?[] Id) {
			return this.Where1Or("a.`Id` = {0}", Id);
		}
		public PostsSelectBuild WhereCount_good(params int?[] Count_good) {
			return this.Where1Or("a.`Count_good` = {0}", Count_good);
		}
		public PostsSelectBuild WhereCount_notgood(params int?[] Count_notgood) {
			return this.Where1Or("a.`Count_notgood` = {0}", Count_notgood);
		}
		public PostsSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`Create_time` >= {0}", begin) as PostsSelectBuild;
		}
		public PostsSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`Create_time` between {0} and {1}", begin, end) as PostsSelectBuild;
		}
		public PostsSelectBuild WhereIndex(params uint?[] Index) {
			return this.Where1Or("a.`Index` = {0}", Index);
		}
		protected new PostsSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as PostsSelectBuild;
		}
		public PostsSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}