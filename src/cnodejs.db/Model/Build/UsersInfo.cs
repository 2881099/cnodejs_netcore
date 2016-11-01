using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace cnodejs.Model {

	public partial class UsersInfo : IQueryable {
		Type IQueryable.ElementType {
			get {
				throw new NotImplementedException();
			}
		}

		Expression IQueryable.Expression {
			get {
				throw new NotImplementedException();
			}
		}

		IQueryProvider IQueryable.Provider {
			get {
				throw new NotImplementedException();
			}
		}
		IEnumerator IEnumerable.GetEnumerator() {
			throw new NotImplementedException();
		}

		#region fields
		private ulong? _Id;
		private DateTime? _Create_time;
		private string _Email;
		private string _Github;
		private string _Location;
		private string _Password;
		private uint? _Point;
		private string _Sign;
		private string _Username;
		private string _Website;
		private string _Weibo;
		#endregion

		public UsersInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Users(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Email == null ? "null" : _Email.Replace("|", StringifySplit), "|",
				_Github == null ? "null" : _Github.Replace("|", StringifySplit), "|",
				_Location == null ? "null" : _Location.Replace("|", StringifySplit), "|",
				_Password == null ? "null" : _Password.Replace("|", StringifySplit), "|",
				_Point == null ? "null" : _Point.ToString(), "|",
				_Sign == null ? "null" : _Sign.Replace("|", StringifySplit), "|",
				_Username == null ? "null" : _Username.Replace("|", StringifySplit), "|",
				_Website == null ? "null" : _Website.Replace("|", StringifySplit), "|",
				_Weibo == null ? "null" : _Weibo.Replace("|", StringifySplit));
		}
		public UsersInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 11, StringSplitOptions.None);
			if (ret.Length != 11) throw new Exception("格式不正确，UsersInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = ulong.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Create_time = new DateTime(long.Parse(ret[1]));
			if (string.Compare("null", ret[2]) != 0) _Email = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) _Github = ret[3].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[4]) != 0) _Location = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) _Password = ret[5].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[6]) != 0) _Point = uint.Parse(ret[6]);
			if (string.Compare("null", ret[7]) != 0) _Sign = ret[7].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[8]) != 0) _Username = ret[8].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[9]) != 0) _Website = ret[9].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[10]) != 0) _Weibo = ret[10].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Email") ? string.Empty : string.Format(", Email : {0}", Email == null ? "null" : string.Format("'{0}'", Email.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Github") ? string.Empty : string.Format(", Github : {0}", Github == null ? "null" : string.Format("'{0}'", Github.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Location") ? string.Empty : string.Format(", Location : {0}", Location == null ? "null" : string.Format("'{0}'", Location.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Password") ? string.Empty : string.Format(", Password : {0}", Password == null ? "null" : string.Format("'{0}'", Password.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Point") ? string.Empty : string.Format(", Point : {0}", Point == null ? "null" : Point.ToString()), 
				__jsonIgnore.ContainsKey("Sign") ? string.Empty : string.Format(", Sign : {0}", Sign == null ? "null" : string.Format("'{0}'", Sign.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Username") ? string.Empty : string.Format(", Username : {0}", Username == null ? "null" : string.Format("'{0}'", Username.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Website") ? string.Empty : string.Format(", Website : {0}", Website == null ? "null" : string.Format("'{0}'", Website.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Weibo") ? string.Empty : string.Format(", Weibo : {0}", Weibo == null ? "null" : string.Format("'{0}'", Weibo.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (!__jsonIgnore.ContainsKey("Email")) ht["Email"] = Email;
			if (!__jsonIgnore.ContainsKey("Github")) ht["Github"] = Github;
			if (!__jsonIgnore.ContainsKey("Location")) ht["Location"] = Location;
			if (!__jsonIgnore.ContainsKey("Password")) ht["Password"] = Password;
			if (!__jsonIgnore.ContainsKey("Point")) ht["Point"] = Point;
			if (!__jsonIgnore.ContainsKey("Sign")) ht["Sign"] = Sign;
			if (!__jsonIgnore.ContainsKey("Username")) ht["Username"] = Username;
			if (!__jsonIgnore.ContainsKey("Website")) ht["Website"] = Website;
			if (!__jsonIgnore.ContainsKey("Weibo")) ht["Weibo"] = Weibo;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(UsersInfo).GetField("JsonIgnore");
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
			UsersInfo item = obj as UsersInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(UsersInfo op1, UsersInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(UsersInfo op1, UsersInfo op2) {
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
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 电子邮件
		/// </summary>
		public string Email {
			get { return _Email; }
			set { _Email = value; }
		}
		/// <summary>
		/// GitHub
		/// </summary>
		public string Github {
			get { return _Github; }
			set { _Github = value; }
		}
		/// <summary>
		/// 所在地点
		/// </summary>
		public string Location {
			get { return _Location; }
			set { _Location = value; }
		}
		public string Password {
			get { return _Password; }
			set { _Password = value; }
		}
		/// <summary>
		/// 积分
		/// </summary>
		public uint? Point {
			get { return _Point; }
			set { _Point = value; }
		}
		/// <summary>
		/// 个性签名
		/// </summary>
		public string Sign {
			get { return _Sign; }
			set { _Sign = value; }
		}
		/// <summary>
		/// 用户名
		/// </summary>
		public string Username {
			get { return _Username; }
			set { _Username = value; }
		}
		/// <summary>
		/// 个人网站
		/// </summary>
		public string Website {
			get { return _Website; }
			set { _Website = value; }
		}
		/// <summary>
		/// 微博
		/// </summary>
		public string Weibo {
			get { return _Weibo; }
			set { _Weibo = value; }
		}
		private List<PostsInfo> _obj_postss;
		public List<PostsInfo> Obj_postss {
			get {
				if (_obj_postss == null) _obj_postss = cnodejs.BLL.Posts.SelectByUsers_id(_Id).Limit(500).ToList();
				return _obj_postss;
			}
		}
		private List<RolesInfo> _obj_roless;
		public List<RolesInfo> Obj_roless {
			get {
				if (_obj_roless == null) _obj_roless = cnodejs.BLL.Roles.SelectByUsers_id(_Id.Value).ToList();
				return _obj_roless;
			}
		}
		private List<TopicsInfo> _obj_owner_topicss;
		public List<TopicsInfo> Obj_owner_topicss {
			get {
				if (_obj_owner_topicss == null) _obj_owner_topicss = cnodejs.BLL.Topics.SelectByOwner_users_id(_Id).Limit(500).ToList();
				return _obj_owner_topicss;
			}
		}
		private List<UserclaimInfo> _obj_userclaims;
		public List<UserclaimInfo> Obj_userclaims {
			get {
				if (_obj_userclaims == null) _obj_userclaims = cnodejs.BLL.Userclaim.SelectByUsers_id(_Id).Limit(500).ToList();
				return _obj_userclaims;
			}
		}
		private List<TopicsInfo> _obj_topicss;
		public List<TopicsInfo> Obj_topicss {
			get {
				if (_obj_topicss == null) _obj_topicss = cnodejs.BLL.Topics.SelectByUsers_id(_Id.Value).ToList();
				return _obj_topicss;
			}
		}
		#endregion

		public cnodejs.DAL.Users.SqlUpdateBuild UpdateDiy {
			get { return cnodejs.BLL.Users.UpdateDiy(this, _Id); }
		}

		public PostsInfo AddPosts(PostsInfo Posts, TopicsInfo Topics, string Content, int? Count_good, int? Count_notgood, DateTime? Create_time, uint? Index) {
			return AddPosts(Posts.Id, Topics.Id, Content, Count_good, Count_notgood, Create_time, Index);
		}
		public PostsInfo AddPosts(ulong? Posts_id, ulong? Topics_id, string Content, int? Count_good, int? Count_notgood, DateTime? Create_time, uint? Index) {
			return cnodejs.BLL.Posts.Insert(new PostsInfo {
				Posts_id = Posts_id, 
				Topics_id = Topics_id, 
				Users_id = this.Id, 
				Content = Content, 
				Count_good = Count_good, 
				Count_notgood = Count_notgood, 
				Create_time = Create_time, 
				Index = Index});
		}

		public Roles_usersInfo FlagRoles(RolesInfo Roles) {
			return FlagRoles(Roles.Id);
		}
		public Roles_usersInfo FlagRoles(uint? Roles_id) {
			Roles_usersInfo item = cnodejs.BLL.Roles_users.GetItem(Roles_id, this.Id);
			if (item == null) item = cnodejs.BLL.Roles_users.Insert(new Roles_usersInfo {
				Roles_id = Roles_id, 
				Users_id = this.Id});
			return item;
		}

		public int UnflagRoles(RolesInfo Roles) {
			return UnflagRoles(Roles.Id);
		}
		public int UnflagRoles(uint? Roles_id) {
			return cnodejs.BLL.Roles_users.Delete(Roles_id, this.Id);
		}
		public int UnflagRolesALL() {
			return cnodejs.BLL.Roles_users.DeleteByUsers_id(this.Id);
		}

		public TopicsInfo AddTopics(PostsInfo Last_posts, int? Count_posts, uint? Count_views, DateTime? Create_time, string Title, ulong? Top, DateTime? Update_time) {
			return AddTopics(Last_posts.Id, Count_posts, Count_views, Create_time, Title, Top, Update_time);
		}
		public TopicsInfo AddTopics(ulong? Last_posts_id, int? Count_posts, uint? Count_views, DateTime? Create_time, string Title, ulong? Top, DateTime? Update_time) {
			return cnodejs.BLL.Topics.Insert(new TopicsInfo {
				Last_posts_id = Last_posts_id, 
				Owner_users_id = this.Id, 
				Count_posts = Count_posts, 
				Count_views = Count_views, 
				Create_time = Create_time, 
				Title = Title, 
				Top = Top, 
				Update_time = Update_time});
		}

		public UserclaimInfo AddUserclaim(DateTime? Create_time, string Type, string Value) {
			return cnodejs.BLL.Userclaim.Insert(new UserclaimInfo {
				Users_id = this.Id, 
				Create_time = Create_time, 
				Type = Type, 
				Value = Value});
		}

		public Users_topicsInfo FlagTopics(TopicsInfo Topics) {
			return FlagTopics(Topics.Id);
		}
		public Users_topicsInfo FlagTopics(ulong? Topics_id) {
			Users_topicsInfo item = cnodejs.BLL.Users_topics.GetItem(Topics_id, this.Id);
			if (item == null) item = cnodejs.BLL.Users_topics.Insert(new Users_topicsInfo {
				Topics_id = Topics_id, 
				Users_id = this.Id});
			return item;
		}

		public int UnflagTopics(TopicsInfo Topics) {
			return UnflagTopics(Topics.Id);
		}
		public int UnflagTopics(ulong? Topics_id) {
			return cnodejs.BLL.Users_topics.Delete(Topics_id, this.Id);
		}
		public int UnflagTopicsALL() {
			return cnodejs.BLL.Users_topics.DeleteByUsers_id(this.Id);
		}

		
	}
}

