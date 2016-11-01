using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.BLL {

	public partial class Roles {

		protected static readonly cnodejs.DAL.Roles dal = new cnodejs.DAL.Roles();
		protected static readonly int itemCacheTimeout;

		static Roles() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT_Roles"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByName(string Name) {
			return dal.DeleteByName(Name);
		}

		public static int Update(RolesInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static cnodejs.DAL.Roles.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static cnodejs.DAL.Roles.SqlUpdateBuild UpdateDiy(RolesInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new cnodejs.DAL.Roles.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static cnodejs.DAL.Roles.SqlUpdateBuild UpdateDiyDangerous {
			get { return new cnodejs.DAL.Roles.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Roles.Insert(RolesInfo item)
		/// </summary>
		[Obsolete]
		public static RolesInfo Insert(string Name) {
			return Insert(new RolesInfo {
				Name = Name});
		}
		public static RolesInfo Insert(RolesInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(RolesInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("cnodejs_BLL_Roles_", item.Id));
			RedisHelper.Remove(string.Concat("cnodejs_BLL_RolesByName_", item.Name));
		}
		#endregion

		public static RolesInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("cnodejs_BLL_Roles_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new RolesInfo(value); } catch { }
			RolesInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}
		public static RolesInfo GetItemByName(string Name) {
			if (Name == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItemByName(Name);
			string key = string.Concat("cnodejs_BLL_RolesByName_", Name);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new RolesInfo(value); } catch { }
			RolesInfo item = dal.GetItemByName(Name);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<RolesInfo> GetItems() {
			return Select.ToList();
		}
		public static RolesSelectBuild Select {
			get { return new RolesSelectBuild(dal); }
		}
		public static RolesSelectBuild SelectByUsers(params UsersInfo[] items) {
			return Select.WhereUsers(items);
		}
		public static RolesSelectBuild SelectByUsers_id(params ulong[] ids) {
			return Select.WhereUsers_id(ids);
		}
	}
	public partial class RolesSelectBuild : SelectBuild<RolesInfo, RolesSelectBuild> {
		public RolesSelectBuild WhereUsers(params UsersInfo[] items) {
			if (items == null) return this;
			return WhereUsers_id(items.Where<UsersInfo>(a => a != null).Select<UsersInfo, ulong>(a => a.Id.Value).ToArray());
		}
		public RolesSelectBuild WhereUsers_id(params ulong[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `roles_id` FROM `roles_users` WHERE `roles_id` = a.`id` AND `users_id` IN ({0}) )", string.Join<ulong>(",", ids))) as RolesSelectBuild;
		}
		public RolesSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`Id` = {0}", Id);
		}
		public RolesSelectBuild WhereName(params string[] Name) {
			return this.Where1Or("a.`Name` = {0}", Name);
		}
		protected new RolesSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as RolesSelectBuild;
		}
		public RolesSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}