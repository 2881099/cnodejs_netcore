using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using cnodejs.Model;

namespace cnodejs.BLL {

	public partial class Users {

		protected static readonly cnodejs.DAL.Users dal = new cnodejs.DAL.Users();
		protected static readonly int itemCacheTimeout;

		static Users() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT_Users"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["cnodejs_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(ulong? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByUsername(string Username) {
			return dal.DeleteByUsername(Username);
		}
		public static int DeleteByEmail(string Email) {
			return dal.DeleteByEmail(Email);
		}

		public static int Update(UsersInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static cnodejs.DAL.Users.SqlUpdateBuild UpdateDiy(ulong? Id) {
			return UpdateDiy(null, Id);
		}
		public static cnodejs.DAL.Users.SqlUpdateBuild UpdateDiy(UsersInfo item, ulong? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new cnodejs.DAL.Users.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static cnodejs.DAL.Users.SqlUpdateBuild UpdateDiyDangerous {
			get { return new cnodejs.DAL.Users.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Users.Insert(UsersInfo item)
		/// </summary>
		[Obsolete]
		public static UsersInfo Insert(DateTime? Create_time, string Email, string Github, string Location, string Password, uint? Point, string Sign, string Username, string Website, string Weibo) {
			return Insert(new UsersInfo {
				Create_time = Create_time, 
				Email = Email, 
				Github = Github, 
				Location = Location, 
				Password = Password, 
				Point = Point, 
				Sign = Sign, 
				Username = Username, 
				Website = Website, 
				Weibo = Weibo});
		}
		public static UsersInfo Insert(UsersInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(UsersInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("cnodejs_BLL_Users_", item.Id));
			RedisHelper.Remove(string.Concat("cnodejs_BLL_UsersByUsername_", item.Username));
			RedisHelper.Remove(string.Concat("cnodejs_BLL_UsersByEmail_", item.Email));
		}
		#endregion

		public static UsersInfo GetItem(ulong? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("cnodejs_BLL_Users_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new UsersInfo(value); } catch { }
			UsersInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}
		public static UsersInfo GetItemByUsername(string Username) {
			if (Username == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItemByUsername(Username);
			string key = string.Concat("cnodejs_BLL_UsersByUsername_", Username);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new UsersInfo(value); } catch { }
			UsersInfo item = dal.GetItemByUsername(Username);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}
		public static UsersInfo GetItemByEmail(string Email) {
			if (Email == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItemByEmail(Email);
			string key = string.Concat("cnodejs_BLL_UsersByEmail_", Email);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new UsersInfo(value); } catch { }
			UsersInfo item = dal.GetItemByEmail(Email);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<UsersInfo> GetItems() {
			return Select.ToList();
		}
		public static UsersSelectBuild Select {
			get { return new UsersSelectBuild(dal); }
		}
		public static UsersSelectBuild SelectByTopics(params TopicsInfo[] items) {
			return Select.WhereTopics(items);
		}
		public static UsersSelectBuild SelectByTopics_id(params ulong[] ids) {
			return Select.WhereTopics_id(ids);
		}
	}
	public partial class UsersSelectBuild : SelectBuild<UsersInfo, UsersSelectBuild> {
		public UsersSelectBuild WhereTopics(params TopicsInfo[] items) {
			if (items == null) return this;
			return WhereTopics_id(items.Where<TopicsInfo>(a => a != null).Select<TopicsInfo, ulong>(a => a.Id.Value).ToArray());
		}
		public UsersSelectBuild WhereTopics_id(params ulong[] ids) {
			if (ids == null || ids.Length == 0) return this;
			return base.Where(string.Format(@"EXISTS( SELECT `users_id` FROM `users_topics` WHERE `users_id` = a.`id` AND `topics_id` IN ({0}) )", string.Join<ulong>(",", ids))) as UsersSelectBuild;
		}
		public UsersSelectBuild WhereId(params ulong?[] Id) {
			return this.Where1Or("a.`Id` = {0}", Id);
		}
		public UsersSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`Create_time` >= {0}", begin) as UsersSelectBuild;
		}
		public UsersSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`Create_time` between {0} and {1}", begin, end) as UsersSelectBuild;
		}
		public UsersSelectBuild WhereEmail(params string[] Email) {
			return this.Where1Or("a.`Email` = {0}", Email);
		}
		public UsersSelectBuild WhereGithub(params string[] Github) {
			return this.Where1Or("a.`Github` = {0}", Github);
		}
		public UsersSelectBuild WhereLocation(params string[] Location) {
			return this.Where1Or("a.`Location` = {0}", Location);
		}
		public UsersSelectBuild WherePassword(params string[] Password) {
			return this.Where1Or("a.`Password` = {0}", Password);
		}
		public UsersSelectBuild WherePoint(params uint?[] Point) {
			return this.Where1Or("a.`Point` = {0}", Point);
		}
		public UsersSelectBuild WhereSign(params string[] Sign) {
			return this.Where1Or("a.`Sign` = {0}", Sign);
		}
		public UsersSelectBuild WhereUsername(params string[] Username) {
			return this.Where1Or("a.`Username` = {0}", Username);
		}
		public UsersSelectBuild WhereWebsite(params string[] Website) {
			return this.Where1Or("a.`Website` = {0}", Website);
		}
		public UsersSelectBuild WhereWeibo(params string[] Weibo) {
			return this.Where1Or("a.`Weibo` = {0}", Weibo);
		}
		protected new UsersSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as UsersSelectBuild;
		}
		public UsersSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}