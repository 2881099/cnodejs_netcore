using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.BLL {

	public partial class Userclaim {

		protected static readonly cnodejs.DAL.Userclaim dal = new cnodejs.DAL.Userclaim();
		protected static readonly int itemCacheTimeout;

		static Userclaim() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT_Userclaim"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(ulong? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByUsers_id(ulong? Users_id) {
			return dal.DeleteByUsers_id(Users_id);
		}

		public static int Update(UserclaimInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static cnodejs.DAL.Userclaim.SqlUpdateBuild UpdateDiy(ulong? Id) {
			return UpdateDiy(null, Id);
		}
		public static cnodejs.DAL.Userclaim.SqlUpdateBuild UpdateDiy(UserclaimInfo item, ulong? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new cnodejs.DAL.Userclaim.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static cnodejs.DAL.Userclaim.SqlUpdateBuild UpdateDiyDangerous {
			get { return new cnodejs.DAL.Userclaim.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Userclaim.Insert(UserclaimInfo item)
		/// </summary>
		[Obsolete]
		public static UserclaimInfo Insert(ulong? Users_id, DateTime? Create_time, string Type, string Value) {
			return Insert(new UserclaimInfo {
				Users_id = Users_id, 
				Create_time = Create_time, 
				Type = Type, 
				Value = Value});
		}
		public static UserclaimInfo Insert(UserclaimInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(UserclaimInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("cnodejs_BLL_Userclaim_", item.Id));
		}
		#endregion

		public static UserclaimInfo GetItem(ulong? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("cnodejs_BLL_Userclaim_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new UserclaimInfo(value); } catch { }
			UserclaimInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<UserclaimInfo> GetItems() {
			return Select.ToList();
		}
		public static UserclaimSelectBuild Select {
			get { return new UserclaimSelectBuild(dal); }
		}
		public static List<UserclaimInfo> GetItemsByUsers_id(params ulong?[] Users_id) {
			return Select.WhereUsers_id(Users_id).ToList();
		}
		public static List<UserclaimInfo> GetItemsByUsers_id(ulong?[] Users_id, int limit) {
			return Select.WhereUsers_id(Users_id).Limit(limit).ToList();
		}
		public static UserclaimSelectBuild SelectByUsers_id(params ulong?[] Users_id) {
			return Select.WhereUsers_id(Users_id);
		}
	}
	public partial class UserclaimSelectBuild : SelectBuild<UserclaimInfo, UserclaimSelectBuild> {
		public UserclaimSelectBuild WhereUsers_id(params ulong?[] Users_id) {
			return this.Where1Or("a.`Users_id` = {0}", Users_id);
		}
		public UserclaimSelectBuild WhereId(params ulong?[] Id) {
			return this.Where1Or("a.`Id` = {0}", Id);
		}
		public UserclaimSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`Create_time` >= {0}", begin) as UserclaimSelectBuild;
		}
		public UserclaimSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`Create_time` between {0} and {1}", begin, end) as UserclaimSelectBuild;
		}
		public UserclaimSelectBuild WhereType(params string[] Type) {
			return this.Where1Or("a.`Type` = {0}", Type);
		}
		public UserclaimSelectBuild WhereValue(params string[] Value) {
			return this.Where1Or("a.`Value` = {0}", Value);
		}
		protected new UserclaimSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as UserclaimSelectBuild;
		}
		public UserclaimSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}