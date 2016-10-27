using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace cnodejs.Model {

	public partial class Users_topicsInfo {
		#region fields
		private ulong? _Topics_id;
		private TopicsInfo _obj_topics;
		private ulong? _Users_id;
		private UsersInfo _obj_users;
		#endregion

		public Users_topicsInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Users_topics(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Topics_id == null ? "null" : _Topics_id.ToString(), "|",
				_Users_id == null ? "null" : _Users_id.ToString());
		}
		public Users_topicsInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，Users_topicsInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Topics_id = ulong.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Users_id = ulong.Parse(ret[1]);
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Topics_id") ? string.Empty : string.Format(", Topics_id : {0}", Topics_id == null ? "null" : Topics_id.ToString()), 
				__jsonIgnore.ContainsKey("Users_id") ? string.Empty : string.Format(", Users_id : {0}", Users_id == null ? "null" : Users_id.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Topics_id")) ht["Topics_id"] = Topics_id;
			if (!__jsonIgnore.ContainsKey("Users_id")) ht["Users_id"] = Users_id;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(Users_topicsInfo).GetField("JsonIgnore");
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
			Users_topicsInfo item = obj as Users_topicsInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(Users_topicsInfo op1, Users_topicsInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(Users_topicsInfo op1, Users_topicsInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
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
		#endregion

		public cnodejs.DAL.Users_topics.SqlUpdateBuild UpdateDiy {
			get { return cnodejs.BLL.Users_topics.UpdateDiy(this, _Topics_id, _Users_id); }
		}
	}
}

