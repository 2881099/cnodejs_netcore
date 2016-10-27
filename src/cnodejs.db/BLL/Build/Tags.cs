using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.BLL {

	public partial class Tags {

		protected static readonly cnodejs.DAL.Tags dal = new cnodejs.DAL.Tags();
		protected static readonly int itemCacheTimeout;

		static Tags() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT_Tags"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByKeyname(string Keyname) {
			return dal.DeleteByKeyname(Keyname);
		}

		public static int Update(TagsInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static cnodejs.DAL.Tags.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static cnodejs.DAL.Tags.SqlUpdateBuild UpdateDiy(TagsInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new cnodejs.DAL.Tags.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static cnodejs.DAL.Tags.SqlUpdateBuild UpdateDiyDangerous {
			get { return new cnodejs.DAL.Tags.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Tags.Insert(TagsInfo item)
		/// </summary>
		[Obsolete]
		public static TagsInfo Insert(DateTime? Create_time, string Keyname, string Name) {
			return Insert(new TagsInfo {
				Create_time = Create_time, 
				Keyname = Keyname, 
				Name = Name});
		}
		public static TagsInfo Insert(TagsInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(TagsInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("cnodejs_BLL_Tags_", item.Id));
			RedisHelper.Remove(string.Concat("cnodejs_BLL_TagsByKeyname_", item.Keyname));
		}
		#endregion

		public static TagsInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("cnodejs_BLL_Tags_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new TagsInfo(value); } catch { }
			TagsInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}
		public static TagsInfo GetItemByKeyname(string Keyname) {
			if (Keyname == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItemByKeyname(Keyname);
			string key = string.Concat("cnodejs_BLL_TagsByKeyname_", Keyname);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new TagsInfo(value); } catch { }
			TagsInfo item = dal.GetItemByKeyname(Keyname);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<TagsInfo> GetItems() {
			return Select.ToList();
		}
		public static TagsSelectBuild Select {
			get { return new TagsSelectBuild(dal); }
		}
		public static TagsSelectBuild SelectByTopics(params TopicsInfo[] items) {
			return Select.WhereTopics(items);
		}
		public static TagsSelectBuild SelectByTopics_id(params ulong[] ids) {
			return Select.WhereTopics_id(ids);
		}
	}
	public partial class TagsSelectBuild : SelectBuild<TagsInfo, TagsSelectBuild> {
		public TagsSelectBuild WhereTopics(params TopicsInfo[] items) {
			if (items == null) return this;
			return WhereTopics_id(items.Where<TopicsInfo>(a => a != null).Select<TopicsInfo, ulong>(a => a.Id.Value).ToArray());
		}
		public TagsSelectBuild WhereTopics_id(params ulong[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `tags_id` FROM `topics_tags` WHERE `tags_id` = a.`id` AND `topics_id` IN ({0}) )", string.Join<ulong>(",", ids))) as TagsSelectBuild;
		}
		public TagsSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`Id` = {0}", Id);
		}
		public TagsSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`Create_time` >= {0}", begin) as TagsSelectBuild;
		}
		public TagsSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`Create_time` between {0} and {1}", begin, end) as TagsSelectBuild;
		}
		public TagsSelectBuild WhereKeyname(params string[] Keyname) {
			return this.Where1Or("a.`Keyname` = {0}", Keyname);
		}
		public TagsSelectBuild WhereName(params string[] Name) {
			return this.Where1Or("a.`Name` = {0}", Name);
		}
		protected new TagsSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as TagsSelectBuild;
		}
		public TagsSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}