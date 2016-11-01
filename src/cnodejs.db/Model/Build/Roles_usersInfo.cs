using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace cnodejs.Model {

	public partial class Roles_usersInfo {
		#region fields
		private uint? _Roles_id;
		private RolesInfo _obj_roles;
		private ulong? _Users_id;
		private UsersInfo _obj_users;
		#endregion

		public Roles_usersInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Roles_users(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Roles_id == null ? "null" : _Roles_id.ToString(), "|",
				_Users_id == null ? "null" : _Users_id.ToString());
		}
		public Roles_usersInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，Roles_usersInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Roles_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Users_id = ulong.Parse(ret[1]);
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Roles_id") ? string.Empty : string.Format(", Roles_id : {0}", Roles_id == null ? "null" : Roles_id.ToString()), 
				__jsonIgnore.ContainsKey("Users_id") ? string.Empty : string.Format(", Users_id : {0}", Users_id == null ? "null" : Users_id.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Roles_id")) ht["Roles_id"] = Roles_id;
			if (!__jsonIgnore.ContainsKey("Users_id")) ht["Users_id"] = Users_id;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(Roles_usersInfo).GetField("JsonIgnore");
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
			Roles_usersInfo item = obj as Roles_usersInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(Roles_usersInfo op1, Roles_usersInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(Roles_usersInfo op1, Roles_usersInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public uint? Roles_id {
			get { return _Roles_id; }
			set {
				if (_Roles_id != value) _obj_roles = null;
				_Roles_id = value;
			}
		}
		public RolesInfo Obj_roles {
			get {
				if (_obj_roles == null) _obj_roles = cnodejs.BLL.Roles.GetItem(_Roles_id);
				return _obj_roles;
			}
			internal set { _obj_roles = value; }
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

		public cnodejs.DAL.Roles_users.SqlUpdateBuild UpdateDiy {
			get { return cnodejs.BLL.Roles_users.UpdateDiy(this, _Roles_id, _Users_id); }
		}
	}
}

