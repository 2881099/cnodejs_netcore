using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace cnodejs.Model {

	public partial class PostsInfo {
		#region fields
		private ulong? _Id;
		private ulong? _Posts_id;
		private PostsInfo _obj_posts;
		private ulong? _Topics_id;
		private TopicsInfo _obj_topics;
		private ulong? _Users_id;
		private UsersInfo _obj_users;
		private string _Content;
		private int? _Count_good;
		private int? _Count_notgood;
		private DateTime? _Create_time;
		private uint? _Index;
		#endregion

		public PostsInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Posts(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Posts_id == null ? "null" : _Posts_id.ToString(), "|",
				_Topics_id == null ? "null" : _Topics_id.ToString(), "|",
				_Users_id == null ? "null" : _Users_id.ToString(), "|",
				_Content == null ? "null" : _Content.Replace("|", StringifySplit), "|",
				_Count_good == null ? "null" : _Count_good.ToString(), "|",
				_Count_notgood == null ? "null" : _Count_notgood.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Index == null ? "null" : _Index.ToString());
		}
		public PostsInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 9, StringSplitOptions.None);
			if (ret.Length != 9) throw new Exception("格式不正确，PostsInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = ulong.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Posts_id = ulong.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Topics_id = ulong.Parse(ret[2]);
			if (string.Compare("null", ret[3]) != 0) _Users_id = ulong.Parse(ret[3]);
			if (string.Compare("null", ret[4]) != 0) _Content = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) _Count_good = int.Parse(ret[5]);
			if (string.Compare("null", ret[6]) != 0) _Count_notgood = int.Parse(ret[6]);
			if (string.Compare("null", ret[7]) != 0) _Create_time = new DateTime(long.Parse(ret[7]));
			if (string.Compare("null", ret[8]) != 0) _Index = uint.Parse(ret[8]);
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Posts_id") ? string.Empty : string.Format(", Posts_id : {0}", Posts_id == null ? "null" : Posts_id.ToString()), 
				__jsonIgnore.ContainsKey("Topics_id") ? string.Empty : string.Format(", Topics_id : {0}", Topics_id == null ? "null" : Topics_id.ToString()), 
				__jsonIgnore.ContainsKey("Users_id") ? string.Empty : string.Format(", Users_id : {0}", Users_id == null ? "null" : Users_id.ToString()), 
				__jsonIgnore.ContainsKey("Content") ? string.Empty : string.Format(", Content : {0}", Content == null ? "null" : string.Format("'{0}'", Content.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Count_good") ? string.Empty : string.Format(", Count_good : {0}", Count_good == null ? "null" : Count_good.ToString()), 
				__jsonIgnore.ContainsKey("Count_notgood") ? string.Empty : string.Format(", Count_notgood : {0}", Count_notgood == null ? "null" : Count_notgood.ToString()), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Index") ? string.Empty : string.Format(", Index : {0}", Index == null ? "null" : Index.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Posts_id")) ht["Posts_id"] = Posts_id;
			if (!__jsonIgnore.ContainsKey("Topics_id")) ht["Topics_id"] = Topics_id;
			if (!__jsonIgnore.ContainsKey("Users_id")) ht["Users_id"] = Users_id;
			if (!__jsonIgnore.ContainsKey("Content")) ht["Content"] = Content;
			if (!__jsonIgnore.ContainsKey("Count_good")) ht["Count_good"] = Count_good;
			if (!__jsonIgnore.ContainsKey("Count_notgood")) ht["Count_notgood"] = Count_notgood;
			if (!__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (!__jsonIgnore.ContainsKey("Index")) ht["Index"] = Index;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(PostsInfo).GetField("JsonIgnore");
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
			PostsInfo item = obj as PostsInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(PostsInfo op1, PostsInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(PostsInfo op1, PostsInfo op2) {
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
		public ulong? Posts_id {
			get { return _Posts_id; }
			set {
				if (_Posts_id != value) _obj_posts = null;
				_Posts_id = value;
			}
		}
		public PostsInfo Obj_posts {
			get {
				if (_obj_posts == null) _obj_posts = cnodejs.BLL.Posts.GetItem(_Posts_id);
				return _obj_posts;
			}
			internal set { _obj_posts = value; }
		}
		/// <summary>
		/// 主题
		/// </summary>
		public ulong? Topics_id {
			get { return _Topics_id; }
			set {
				if (_Topics_id != value) _obj_topics = null;
				_Topics_id = value;
			}
		}
		public TopicsInfo Obj_topics {
			get {
				if (_obj_topics == null) _obj_topics = cnodejs.BLL.Topics.GetItem(_Topics_id);
				return _obj_topics;
			}
			internal set { _obj_topics = value; }
		}
		/// <summary>
		/// 作者
		/// </summary>
		public ulong? Users_id {
			get { return _Users_id; }
			set {
				if (_Users_id != value) _obj_users = null;
				_Users_id = value;
			}
		}
		public UsersInfo Obj_users {
			get {
				if (_obj_users == null) _obj_users = cnodejs.BLL.Users.GetItem(_Users_id);
				return _obj_users;
			}
			internal set { _obj_users = value; }
		}
		/// <summary>
		/// 内容
		/// </summary>
		public string Content {
			get { return _Content; }
			set { _Content = value; }
		}
		/// <summary>
		/// 顶
		/// </summary>
		public int? Count_good {
			get { return _Count_good; }
			set { _Count_good = value; }
		}
		/// <summary>
		/// 踩
		/// </summary>
		public int? Count_notgood {
			get { return _Count_notgood; }
			set { _Count_notgood = value; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 楼层
		/// </summary>
		public uint? Index {
			get { return _Index; }
			set { _Index = value; }
		}
		private List<PostsInfo> _obj_postss;
		public List<PostsInfo> Obj_postss {
			get {
				if (_obj_postss == null) _obj_postss = cnodejs.BLL.Posts.SelectByPosts_id(_Id).Limit(500).ToList();
				return _obj_postss;
			}
		}
		private List<TopicsInfo> _obj_last_topicss;
		public List<TopicsInfo> Obj_last_topicss {
			get {
				if (_obj_last_topicss == null) _obj_last_topicss = cnodejs.BLL.Topics.SelectByLast_posts_id(_Id).Limit(500).ToList();
				return _obj_last_topicss;
			}
		}
		#endregion

		public cnodejs.DAL.Posts.SqlUpdateBuild UpdateDiy {
			get { return cnodejs.BLL.Posts.UpdateDiy(this, _Id); }
		}
		public PostsInfo AddPosts(TopicsInfo Topics, UsersInfo Users, string Content, int? Count_good, int? Count_notgood, DateTime? Create_time, uint? Index) {
			return AddPosts(Topics.Id, Users.Id, Content, Count_good, Count_notgood, Create_time, Index);
		}
		public PostsInfo AddPosts(ulong? Topics_id, ulong? Users_id, string Content, int? Count_good, int? Count_notgood, DateTime? Create_time, uint? Index) {
			return cnodejs.BLL.Posts.Insert(new PostsInfo {
				Posts_id = this.Id, 
				Topics_id = Topics_id, 
				Users_id = Users_id, 
				Content = Content, 
				Count_good = Count_good, 
				Count_notgood = Count_notgood, 
				Create_time = Create_time, 
				Index = Index});
		}

		public TopicsInfo AddTopics(UsersInfo Owner_users, int? Count_posts, uint? Count_views, DateTime? Create_time, string Title, ulong? Top, DateTime? Update_time) {
			return AddTopics(Owner_users.Id, Count_posts, Count_views, Create_time, Title, Top, Update_time);
		}
		public TopicsInfo AddTopics(ulong? Owner_users_id, int? Count_posts, uint? Count_views, DateTime? Create_time, string Title, ulong? Top, DateTime? Update_time) {
			return cnodejs.BLL.Topics.Insert(new TopicsInfo {
				Last_posts_id = this.Id, 
				Owner_users_id = Owner_users_id, 
				Count_posts = Count_posts, 
				Count_views = Count_views, 
				Create_time = Create_time, 
				Title = Title, 
				Top = Top, 
				Update_time = Update_time});
		}

	}
}

