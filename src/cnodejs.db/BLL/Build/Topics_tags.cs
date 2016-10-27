using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.BLL {

	public partial class Topics_tags {

		protected static readonly cnodejs.DAL.Topics_tags dal = new cnodejs.DAL.Topics_tags();
		protected static readonly int itemCacheTimeout;

		static Topics_tags() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT_Topics_tags"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Tags_id, ulong? Topics_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Tags_id, Topics_id));
			return dal.Delete(Tags_id, Topics_id);
		}
		public static int DeleteByTopics_id(ulong? Topics_id) {
			return dal.DeleteByTopics_id(Topics_id);
		}
		public static int DeleteByTags_id(uint? Tags_id) {
			return dal.DeleteByTags_id(Tags_id);
		}

		public static int Update(Topics_tagsInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static cnodejs.DAL.Topics_tags.SqlUpdateBuild UpdateDiy(uint? Tags_id, ulong? Topics_id) {
			return UpdateDiy(null, Tags_id, Topics_id);
		}
		public static cnodejs.DAL.Topics_tags.SqlUpdateBuild UpdateDiy(Topics_tagsInfo item, uint? Tags_id, ulong? Topics_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Tags_id, Topics_id));
			return new cnodejs.DAL.Topics_tags.SqlUpdateBuild(item, Tags_id, Topics_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static cnodejs.DAL.Topics_tags.SqlUpdateBuild UpdateDiyDangerous {
			get { return new cnodejs.DAL.Topics_tags.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Topics_tags.Insert(Topics_tagsInfo item)
		/// </summary>
		[Obsolete]
		public static Topics_tagsInfo Insert(uint? Tags_id, ulong? Topics_id) {
			return Insert(new Topics_tagsInfo {
				Tags_id = Tags_id, 
				Topics_id = Topics_id});
		}
		public static Topics_tagsInfo Insert(Topics_tagsInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Topics_tagsInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("cnodejs_BLL_Topics_tags_", item.Tags_id, "_,_", item.Topics_id));
		}
		#endregion

		public static Topics_tagsInfo GetItem(uint? Tags_id, ulong? Topics_id) {
			if (Tags_id == null || Topics_id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Tags_id, Topics_id);
			string key = string.Concat("cnodejs_BLL_Topics_tags_", Tags_id, "_,_", Topics_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new Topics_tagsInfo(value); } catch { }
			Topics_tagsInfo item = dal.GetItem(Tags_id, Topics_id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Topics_tagsInfo> GetItems() {
			return Select.ToList();
		}
		public static Topics_tagsSelectBuild Select {
			get { return new Topics_tagsSelectBuild(dal); }
		}
		public static List<Topics_tagsInfo> GetItemsByTopics_id(params ulong?[] Topics_id) {
			return Select.WhereTopics_id(Topics_id).ToList();
		}
		public static List<Topics_tagsInfo> GetItemsByTopics_id(ulong?[] Topics_id, int limit) {
			return Select.WhereTopics_id(Topics_id).Limit(limit).ToList();
		}
		public static Topics_tagsSelectBuild SelectByTopics_id(params ulong?[] Topics_id) {
			return Select.WhereTopics_id(Topics_id);
		}
		public static List<Topics_tagsInfo> GetItemsByTags_id(params uint?[] Tags_id) {
			return Select.WhereTags_id(Tags_id).ToList();
		}
		public static List<Topics_tagsInfo> GetItemsByTags_id(uint?[] Tags_id, int limit) {
			return Select.WhereTags_id(Tags_id).Limit(limit).ToList();
		}
		public static Topics_tagsSelectBuild SelectByTags_id(params uint?[] Tags_id) {
			return Select.WhereTags_id(Tags_id);
		}
	}
	public partial class Topics_tagsSelectBuild : SelectBuild<Topics_tagsInfo, Topics_tagsSelectBuild> {
		public Topics_tagsSelectBuild WhereTopics_id(params ulong?[] Topics_id) {
			return this.Where1Or("a.`Topics_id` = {0}", Topics_id);
		}
		public Topics_tagsSelectBuild WhereTags_id(params uint?[] Tags_id) {
			return this.Where1Or("a.`Tags_id` = {0}", Tags_id);
		}
		protected new Topics_tagsSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Topics_tagsSelectBuild;
		}
		public Topics_tagsSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}