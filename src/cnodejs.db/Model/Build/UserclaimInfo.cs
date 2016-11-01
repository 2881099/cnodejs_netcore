using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace cnodejs.Model {

	public partial class UserclaimInfo {
		#region fields
		private ulong? _Id;
		private ulong? _Users_id;
		private UsersInfo _obj_users;
		private DateTime? _Create_time;
		private string _Type;
		private string _Value;
		#endregion

		public UserclaimInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Userclaim(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Users_id == null ? "null" : _Users_id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Type == null ? "null" : _Type.Replace("|", StringifySplit), "|",
				_Value == null ? "null" : _Value.Replace("|", StringifySplit));
		}
		public UserclaimInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 5, StringSplitOptions.None);
			if (ret.Length != 5) throw new Exception("格式不正确，UserclaimInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = ulong.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Users_id = ulong.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Create_time = new DateTime(long.Parse(ret[2]));
			if (string.Compare("null", ret[3]) != 0) _Type = ret[3].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[4]) != 0) _Value = ret[4].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Users_id") ? string.Empty : string.Format(", Users_id : {0}", Users_id == null ? "null" : Users_id.ToString()), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Type") ? string.Empty : string.Format(", Type : {0}", Type == null ? "null" : string.Format("'{0}'", Type.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Value") ? string.Empty : string.Format(", Value : {0}", Value == null ? "null" : string.Format("'{0}'", Value.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Users_id")) ht["Users_id"] = Users_id;
			if (!__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (!__jsonIgnore.ContainsKey("Type")) ht["Type"] = Type;
			if (!__jsonIgnore.ContainsKey("Value")) ht["Value"] = Value;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(UserclaimInfo).GetField("JsonIgnore");
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
			UserclaimInfo item = obj as UserclaimInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(UserclaimInfo op1, UserclaimInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(UserclaimInfo op1, UserclaimInfo op2) {
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
		/// 用户
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
		/// 创建时间
		/// </summary>
		public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 类型
		/// </summary>
		public string Type {
			get { return _Type; }
			set { _Type = value; }
		}
		/// <summary>
		/// 值
		/// </summary>
		public string Value {
			get { return _Value; }
			set { _Value = value; }
		}
		#endregion

		public cnodejs.DAL.Userclaim.SqlUpdateBuild UpdateDiy {
			get { return cnodejs.BLL.Userclaim.UpdateDiy(this, _Id); }
		}
	}
}

