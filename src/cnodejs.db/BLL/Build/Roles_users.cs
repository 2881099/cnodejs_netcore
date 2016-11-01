using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.BLL {

	public partial class Roles_users {

		protected static readonly cnodejs.DAL.Roles_users dal = new cnodejs.DAL.Roles_users();
		protected static readonly int itemCacheTimeout;

		static Roles_users() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT_Roles_users"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Roles_id, ulong? Users_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Roles_id, Users_id));
			return dal.Delete(Roles_id, Users_id);
		}
		public static int DeleteByUsers_id(ulong? Users_id) {
			return dal.DeleteByUsers_id(Users_id);
		}
		public static int DeleteByRoles_id(uint? Roles_id) {
			return dal.DeleteByRoles_id(Roles_id);
		}

		public static int Update(Roles_usersInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static cnodejs.DAL.Roles_users.SqlUpdateBuild UpdateDiy(uint? Roles_id, ulong? Users_id) {
			return UpdateDiy(null, Roles_id, Users_id);
		}
		public static cnodejs.DAL.Roles_users.SqlUpdateBuild UpdateDiy(Roles_usersInfo item, uint? Roles_id, ulong? Users_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Roles_id, Users_id));
			return new cnodejs.DAL.Roles_users.SqlUpdateBuild(item, Roles_id, Users_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static cnodejs.DAL.Roles_users.SqlUpdateBuild UpdateDiyDangerous {
			get { return new cnodejs.DAL.Roles_users.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Roles_users.Insert(Roles_usersInfo item)
		/// </summary>
		[Obsolete]
		public static Roles_usersInfo Insert(uint? Roles_id, ulong? Users_id) {
			return Insert(new Roles_usersInfo {
				Roles_id = Roles_id, 
				Users_id = Users_id});
		}
		public static Roles_usersInfo Insert(Roles_usersInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Roles_usersInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("cnodejs_BLL_Roles_users_", item.Roles_id, "_,_", item.Users_id));
		}
		#endregion

		public static Roles_usersInfo GetItem(uint? Roles_id, ulong? Users_id) {
			if (Roles_id == null || Users_id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Roles_id, Users_id);
			string key = string.Concat("cnodejs_BLL_Roles_users_", Roles_id, "_,_", Users_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new Roles_usersInfo(value); } catch { }
			Roles_usersInfo item = dal.GetItem(Roles_id, Users_id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Roles_usersInfo> GetItems() {
			return Select.ToList();
		}
		public static Roles_usersSelectBuild Select {
			get { return new Roles_usersSelectBuild(dal); }
		}
		public static List<Roles_usersInfo> GetItemsByUsers_id(params ulong?[] Users_id) {
			return Select.WhereUsers_id(Users_id).ToList();
		}
		public static List<Roles_usersInfo> GetItemsByUsers_id(ulong?[] Users_id, int limit) {
			return Select.WhereUsers_id(Users_id).Limit(limit).ToList();
		}
		public static Roles_usersSelectBuild SelectByUsers_id(params ulong?[] Users_id) {
			return Select.WhereUsers_id(Users_id);
		}
		public static List<Roles_usersInfo> GetItemsByRoles_id(params uint?[] Roles_id) {
			return Select.WhereRoles_id(Roles_id).ToList();
		}
		public static List<Roles_usersInfo> GetItemsByRoles_id(uint?[] Roles_id, int limit) {
			return Select.WhereRoles_id(Roles_id).Limit(limit).ToList();
		}
		public static Roles_usersSelectBuild SelectByRoles_id(params uint?[] Roles_id) {
			return Select.WhereRoles_id(Roles_id);
		}
	}
	public partial class Roles_usersSelectBuild : SelectBuild<Roles_usersInfo, Roles_usersSelectBuild> {
		public Roles_usersSelectBuild WhereUsers_id(params ulong?[] Users_id) {
			return this.Where1Or("a.`Users_id` = {0}", Users_id);
		}
		public Roles_usersSelectBuild WhereRoles_id(params uint?[] Roles_id) {
			return this.Where1Or("a.`Roles_id` = {0}", Roles_id);
		}
		protected new Roles_usersSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Roles_usersSelectBuild;
		}
		public Roles_usersSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}