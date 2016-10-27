using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.BLL {

	public partial class Sysdoc {

		protected static readonly cnodejs.DAL.Sysdoc dal = new cnodejs.DAL.Sysdoc();
		protected static readonly int itemCacheTimeout;

		static Sysdoc() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT_Sysdoc"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}

		public static int Update(SysdocInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static cnodejs.DAL.Sysdoc.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static cnodejs.DAL.Sysdoc.SqlUpdateBuild UpdateDiy(SysdocInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new cnodejs.DAL.Sysdoc.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static cnodejs.DAL.Sysdoc.SqlUpdateBuild UpdateDiyDangerous {
			get { return new cnodejs.DAL.Sysdoc.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Sysdoc.Insert(SysdocInfo item)
		/// </summary>
		[Obsolete]
		public static SysdocInfo Insert(string Content, DateTime? Create_time, string Title) {
			return Insert(new SysdocInfo {
				Content = Content, 
				Create_time = Create_time, 
				Title = Title});
		}
		public static SysdocInfo Insert(SysdocInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(SysdocInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("cnodejs_BLL_Sysdoc_", item.Id));
		}
		#endregion

		public static SysdocInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("cnodejs_BLL_Sysdoc_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new SysdocInfo(value); } catch { }
			SysdocInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<SysdocInfo> GetItems() {
			return Select.ToList();
		}
		public static SysdocSelectBuild Select {
			get { return new SysdocSelectBuild(dal); }
		}
	}
	public partial class SysdocSelectBuild : SelectBuild<SysdocInfo, SysdocSelectBuild> {
		public SysdocSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`Id` = {0}", Id);
		}
		public SysdocSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`Create_time` >= {0}", begin) as SysdocSelectBuild;
		}
		public SysdocSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`Create_time` between {0} and {1}", begin, end) as SysdocSelectBuild;
		}
		public SysdocSelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`Title` = {0}", Title);
		}
		protected new SysdocSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as SysdocSelectBuild;
		}
		public SysdocSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}