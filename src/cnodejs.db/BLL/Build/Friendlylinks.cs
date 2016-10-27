using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.BLL {

	public partial class Friendlylinks {

		protected static readonly cnodejs.DAL.Friendlylinks dal = new cnodejs.DAL.Friendlylinks();
		protected static readonly int itemCacheTimeout;

		static Friendlylinks() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT_Friendlylinks"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}

		public static int Update(FriendlylinksInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static cnodejs.DAL.Friendlylinks.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static cnodejs.DAL.Friendlylinks.SqlUpdateBuild UpdateDiy(FriendlylinksInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new cnodejs.DAL.Friendlylinks.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static cnodejs.DAL.Friendlylinks.SqlUpdateBuild UpdateDiyDangerous {
			get { return new cnodejs.DAL.Friendlylinks.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Friendlylinks.Insert(FriendlylinksInfo item)
		/// </summary>
		[Obsolete]
		public static FriendlylinksInfo Insert(DateTime? Create_time, string Link, string Logo, uint? Sort, string Title) {
			return Insert(new FriendlylinksInfo {
				Create_time = Create_time, 
				Link = Link, 
				Logo = Logo, 
				Sort = Sort, 
				Title = Title});
		}
		public static FriendlylinksInfo Insert(FriendlylinksInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(FriendlylinksInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("cnodejs_BLL_Friendlylinks_", item.Id));
		}
		#endregion

		public static FriendlylinksInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("cnodejs_BLL_Friendlylinks_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new FriendlylinksInfo(value); } catch { }
			FriendlylinksInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<FriendlylinksInfo> GetItems() {
			return Select.ToList();
		}
		public static FriendlylinksSelectBuild Select {
			get { return new FriendlylinksSelectBuild(dal); }
		}
	}
	public partial class FriendlylinksSelectBuild : SelectBuild<FriendlylinksInfo, FriendlylinksSelectBuild> {
		public FriendlylinksSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`Id` = {0}", Id);
		}
		public FriendlylinksSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`Create_time` >= {0}", begin) as FriendlylinksSelectBuild;
		}
		public FriendlylinksSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`Create_time` between {0} and {1}", begin, end) as FriendlylinksSelectBuild;
		}
		public FriendlylinksSelectBuild WhereLink(params string[] Link) {
			return this.Where1Or("a.`Link` = {0}", Link);
		}
		public FriendlylinksSelectBuild WhereLogo(params string[] Logo) {
			return this.Where1Or("a.`Logo` = {0}", Logo);
		}
		public FriendlylinksSelectBuild WhereSort(params uint?[] Sort) {
			return this.Where1Or("a.`Sort` = {0}", Sort);
		}
		public FriendlylinksSelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`Title` = {0}", Title);
		}
		protected new FriendlylinksSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as FriendlylinksSelectBuild;
		}
		public FriendlylinksSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}