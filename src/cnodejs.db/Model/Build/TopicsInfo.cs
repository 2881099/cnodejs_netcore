using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace cnodejs.Model {

	public partial class TopicsInfo {
		#region fields
		private ulong? _Id;
		private ulong? _Last_posts_id;
		private PostsInfo _obj_last_posts;
		private ulong? _Owner_users_id;
		private UsersInfo _obj_owner_users;
		private int? _Count_posts;
		private uint? _Count_views;
		private DateTime? _Create_time;
		private string _Title;
		private ulong? _Top;
		private DateTime? _Update_time;
		#endregion

		public TopicsInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Topics(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Last_posts_id == null ? "null" : _Last_posts_id.ToString(), "|",
				_Owner_users_id == null ? "null" : _Owner_users_id.ToString(), "|",
				_Count_posts == null ? "null" : _Count_posts.ToString(), "|",
				_Count_views == null ? "null" : _Count_views.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit), "|",
				_Top == null ? "null" : _Top.ToString(), "|",
				_Update_time == null ? "null" : _Update_time.Value.Ticks.ToString());
		}
		public TopicsInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 9, StringSplitOptions.None);
			if (ret.Length != 9) throw new Exception("格式不正确，TopicsInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = ulong.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Last_posts_id = ulong.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Owner_users_id = ulong.Parse(ret[2]);
			if (string.Compare("null", ret[3]) != 0) _Count_posts = int.Parse(ret[3]);
			if (string.Compare("null", ret[4]) != 0) _Count_views = uint.Parse(ret[4]);
			if (string.Compare("null", ret[5]) != 0) _Create_time = new DateTime(long.Parse(ret[5]));
			if (string.Compare("null", ret[6]) != 0) _Title = ret[6].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[7]) != 0) _Top = ulong.Parse(ret[7]);
			if (string.Compare("null", ret[8]) != 0) _Update_time = new DateTime(long.Parse(ret[8]));
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Last_posts_id") ? string.Empty : string.Format(", Last_posts_id : {0}", Last_posts_id == null ? "null" : Last_posts_id.ToString()), 
				__jsonIgnore.ContainsKey("Owner_users_id") ? string.Empty : string.Format(", Owner_users_id : {0}", Owner_users_id == null ? "null" : Owner_users_id.ToString()), 
				__jsonIgnore.ContainsKey("Count_posts") ? string.Empty : string.Format(", Count_posts : {0}", Count_posts == null ? "null" : Count_posts.ToString()), 
				__jsonIgnore.ContainsKey("Count_views") ? string.Empty : string.Format(", Count_views : {0}", Count_views == null ? "null" : Count_views.ToString()), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Top") ? string.Empty : string.Format(", Top : {0}", Top == null ? "null" : Top.ToString()), 
				__jsonIgnore.ContainsKey("Update_time") ? string.Empty : string.Format(", Update_time : {0}", Update_time == null ? "null" : Update_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Last_posts_id")) ht["Last_posts_id"] = Last_posts_id;
			if (!__jsonIgnore.ContainsKey("Owner_users_id")) ht["Owner_users_id"] = Owner_users_id;
			if (!__jsonIgnore.ContainsKey("Count_posts")) ht["Count_posts"] = Count_posts;
			if (!__jsonIgnore.ContainsKey("Count_views")) ht["Count_views"] = Count_views;
			if (!__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (!__jsonIgnore.ContainsKey("Title")) ht["Title"] = Title;
			if (!__jsonIgnore.ContainsKey("Top")) ht["Top"] = Top;
			if (!__jsonIgnore.ContainsKey("Update_time")) ht["Update_time"] = Update_time;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(TopicsInfo).GetField("JsonIgnore");
						__jsonIgnore = new Dictionary<string, bool>();
						if (field != null) {
							string[] fs = string.Concat(field.GetValue(null)).Split(',');
							foreach (string f in fs) if (!string.IsNullOrEmpty(f)) __jsonIgnore[f] = true;
						}
					}
				}
			}
		}
		public override bool Equals(object obj) {
			TopicsInfo item = obj as TopicsInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(TopicsInfo op1, TopicsInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(TopicsInfo op1, TopicsInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public ulong? Id {
			get { return _Id; }
			set { _Id = value; }
		}
		public ulong? Last_posts_id {
			get { return _Last_posts_id; }
			set {
				if (_Last_posts_id != value) _obj_last_posts = null;
				_Last_posts_id = value;
			}
		}
		public PostsInfo Obj_last_posts {
			get {
				if (_obj_last_posts == null) _obj_last_posts = cnodejs.BLL.Posts.GetItem(_Last_posts_id);
				return _obj_last_posts;
			}
			internal set { _obj_last_posts = value; }
		}
		/// <summary>
		/// 作者
		/// </summary>
		public ulong? Owner_users_id {
			get { return _Owner_users_id; }
			set {
				if (_Owner_users_id != value) _obj_owner_users = null;
				_Owner_users_id = value;
			}
		}
		public UsersInfo Obj_owner_users {
			get {
				if (_obj_owner_users == null) _obj_owner_users = cnodejs.BLL.Users.GetItem(_Owner_users_id);
				return _obj_owner_users;
			}
			internal set { _obj_owner_users = value; }
		}
		/// <summary>
		/// 回复数
		/// </summary>
		public int? Count_posts {
			get { return _Count_posts; }
			set { _Count_posts = value; }
		}
		/// <summary>
		/// 浏览数
		/// </summary>
		public uint? Count_views {
			get { return _Count_views; }
			set { _Count_views = value; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 标题
		/// </summary>
		public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		/// <summary>
		/// 排序(置顶)
		/// </summary>
		public ulong? Top {
			get { return _Top; }
			set { _Top = value; }
		}
		public DateTime? Update_time {
			get { return _Update_time; }
			set { _Update_time = value; }
		}
		private List<PostsInfo> _obj_postss;
		public List<PostsInfo> Obj_postss {
			get {
				if (_obj_postss == null) _obj_postss = cnodejs.BLL.Posts.SelectByTopics_id(_Id).Limit(500).ToList();
				return _obj_postss;
			}
		}
		private List<TagsInfo> _obj_tagss;
		public List<TagsInfo> Obj_tagss {
			get {
				if (_obj_tagss == null) _obj_tagss = cnodejs.BLL.Tags.SelectByTopics_id(_Id.Value).ToList();
				return _obj_tagss;
			}
		}
		private List<UsersInfo> _obj_userss;
		public List<UsersInfo> Obj_userss {
			get {
				if (_obj_userss == null) _obj_userss = cnodejs.BLL.Users.SelectByTopics_id(_Id.Value).ToList();
				return _obj_userss;
			}
		}
		#endregion

		public cnodejs.DAL.Topics.SqlUpdateBuild UpdateDiy {
			get { return cnodejs.BLL.Topics.UpdateDiy(this, _Id); }
		}
		public PostsInfo AddPosts(PostsInfo Posts, UsersInfo Users, string Content, int? Count_good, int? Count_notgood, DateTime? Create_time, uint? Index) {
			return AddPosts(Posts.Id, Users.Id, Content, Count_good, Count_notgood, Create_time, Index);
		}
		public PostsInfo AddPosts(ulong? Posts_id, ulong? Users_id, string Content, int? Count_good, int? Count_notgood, DateTime? Create_time, uint? Index) {
			return cnodejs.BLL.Posts.Insert(new PostsInfo {
				Posts_id = Posts_id, 
				Topics_id = this.Id, 
				Users_id = Users_id, 
				Content = Content, 
				Count_good = Count_good, 
				Count_notgood = Count_notgood, 
				Create_time = Create_time, 
				Index = Index});
		}

		public Topics_tagsInfo FlagTags(TagsInfo Tags) {
			return FlagTags(Tags.Id);
		}
		public Topics_tagsInfo FlagTags(uint? Tags_id) {
			Topics_tagsInfo item = cnodejs.BLL.Topics_tags.GetItem(Tags_id, this.Id);
			if (item == null) item = cnodejs.BLL.Topics_tags.Insert(new Topics_tagsInfo {
				Tags_id = Tags_id, 
				Topics_id = this.Id});
			return item;
		}

		public int UnflagTags(TagsInfo Tags) {
			return UnflagTags(Tags.Id);
		}
		public int UnflagTags(uint? Tags_id) {
			return cnodejs.BLL.Topics_tags.Delete(Tags_id, this.Id);
		}
		public int UnflagTagsALL() {
			return cnodejs.BLL.Topics_tags.DeleteByTopics_id(this.Id);
		}

		public Users_topicsInfo FlagUsers(UsersInfo Users) {
			return FlagUsers(Users.Id);
		}
		public Users_topicsInfo FlagUsers(ulong? Users_id) {
			Users_topicsInfo item = cnodejs.BLL.Users_topics.GetItem(this.Id, Users_id);
			if (item == null) item = cnodejs.BLL.Users_topics.Insert(new Users_topicsInfo {
				Topics_id = this.Id, 
				Users_id = Users_id});
			return item;
		}

		public int UnflagUsers(UsersInfo Users) {
			return UnflagUsers(Users.Id);
		}
		public int UnflagUsers(ulong? Users_id) {
			return cnodejs.BLL.Users_topics.Delete(this.Id, Users_id);
		}
		public int UnflagUsersALL() {
			return cnodejs.BLL.Users_topics.DeleteByTopics_id(this.Id);
		}

	}
}

