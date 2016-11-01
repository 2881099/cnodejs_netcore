using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.BLL {

	public partial class Roleclaim {

		protected static readonly cnodejs.DAL.Roleclaim dal = new cnodejs.DAL.Roleclaim();
		protected static readonly int itemCacheTimeout;

		static Roleclaim() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT_Roleclaim"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByRoles_id(uint? Roles_id) {
			return dal.DeleteByRoles_id(Roles_id);
		}

		public static int Update(RoleclaimInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static cnodejs.DAL.Roleclaim.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static cnodejs.DAL.Roleclaim.SqlUpdateBuild UpdateDiy(RoleclaimInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new cnodejs.DAL.Roleclaim.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static cnodejs.DAL.Roleclaim.SqlUpdateBuild UpdateDiyDangerous {
			get { return new cnodejs.DAL.Roleclaim.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Roleclaim.Insert(RoleclaimInfo item)
		/// </summary>
		[Obsolete]
		public static RoleclaimInfo Insert(uint? Roles_id, DateTime? Create_time, string Type, string Value) {
			return Insert(new RoleclaimInfo {
				Roles_id = Roles_id, 
				Create_time = Create_time, 
				Type = Type, 
				Value = Value});
		}
		public static RoleclaimInfo Insert(RoleclaimInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(RoleclaimInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("cnodejs_BLL_Roleclaim_", item.Id));
		}
		#endregion

		public static RoleclaimInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("cnodejs_BLL_Roleclaim_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new RoleclaimInfo(value); } catch { }
			RoleclaimInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<RoleclaimInfo> GetItems() {
			return Select.ToList();
		}
		public static RoleclaimSelectBuild Select {
			get { return new RoleclaimSelectBuild(dal); }
		}
		public static List<RoleclaimInfo> GetItemsByRoles_id(params uint?[] Roles_id) {
			return Select.WhereRoles_id(Roles_id).ToList();
		}
		public static List<RoleclaimInfo> GetItemsByRoles_id(uint?[] Roles_id, int limit) {
			return Select.WhereRoles_id(Roles_id).Limit(limit).ToList();
		}
		public static RoleclaimSelectBuild SelectByRoles_id(params uint?[] Roles_id) {
			return Select.WhereRoles_id(Roles_id);
		}
	}
	public partial class RoleclaimSelectBuild : SelectBuild<RoleclaimInfo, RoleclaimSelectBuild> {
		public RoleclaimSelectBuild WhereRoles_id(params uint?[] Roles_id) {
			return this.Where1Or("a.`Roles_id` = {0}", Roles_id);
		}
		public RoleclaimSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`Id` = {0}", Id);
		}
		public RoleclaimSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`Create_time` >= {0}", begin) as RoleclaimSelectBuild;
		}
		public RoleclaimSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`Create_time` between {0} and {1}", begin, end) as RoleclaimSelectBuild;
		}
		public RoleclaimSelectBuild WhereType(params string[] Type) {
			return this.Where1Or("a.`Type` = {0}", Type);
		}
		public RoleclaimSelectBuild WhereValue(params string[] Value) {
			return this.Where1Or("a.`Value` = {0}", Value);
		}
		protected new RoleclaimSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as RoleclaimSelectBuild;
		}
		public RoleclaimSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}