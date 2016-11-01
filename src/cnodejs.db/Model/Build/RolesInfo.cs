using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace cnodejs.Model {

	public partial class RolesInfo {
		#region fields
		private uint? _Id;
		private string _Name;
		#endregion

		public RolesInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Roles(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Name == null ? "null" : _Name.Replace("|", StringifySplit));
		}
		public RolesInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，RolesInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Name = ret[1].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Name") ? string.Empty : string.Format(", Name : {0}", Name == null ? "null" : string.Format("'{0}'", Name.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Name")) ht["Name"] = Name;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(RolesInfo).GetField("JsonIgnore");
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
			RolesInfo item = obj as RolesInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(RolesInfo op1, RolesInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(RolesInfo op1, RolesInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public uint? Id {
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		/// 角色
		/// </summary>
		public string Name {
			get { return _Name; }
			set { _Name = value; }
		}
		private List<RoleclaimInfo> _obj_roleclaims;
		public List<RoleclaimInfo> Obj_roleclaims {
			get {
				if (_obj_roleclaims == null) _obj_roleclaims = cnodejs.BLL.Roleclaim.SelectByRoles_id(_Id).Limit(500).ToList();
				return _obj_roleclaims;
			}
		}
		private List<UsersInfo> _obj_userss;
		public List<UsersInfo> Obj_userss {
			get {
				if (_obj_userss == null) _obj_userss = cnodejs.BLL.Users.SelectByRoles_id(_Id.Value).ToList();
				return _obj_userss;
			}
		}
		#endregion

		public cnodejs.DAL.Roles.SqlUpdateBuild UpdateDiy {
			get { return cnodejs.BLL.Roles.UpdateDiy(this, _Id); }
		}
		public RoleclaimInfo AddRoleclaim(DateTime? Create_time, string Type, string Value) {
			return cnodejs.BLL.Roleclaim.Insert(new RoleclaimInfo {
				Roles_id = this.Id, 
				Create_time = Create_time, 
				Type = Type, 
				Value = Value});
		}

		public Roles_usersInfo FlagUsers(UsersInfo Users) {
			return FlagUsers(Users.Id);
		}
		public Roles_usersInfo FlagUsers(ulong? Users_id) {
			Roles_usersInfo item = cnodejs.BLL.Roles_users.GetItem(this.Id, Users_id);
			if (item == null) item = cnodejs.BLL.Roles_users.Insert(new Roles_usersInfo {
				Roles_id = this.Id, 
				Users_id = Users_id});
			return item;
		}

		public int UnflagUsers(UsersInfo Users) {
			return UnflagUsers(Users.Id);
		}
		public int UnflagUsers(ulong? Users_id) {
			return cnodejs.BLL.Roles_users.Delete(this.Id, Users_id);
		}
		public int UnflagUsersALL() {
			return cnodejs.BLL.Roles_users.DeleteByRoles_id(this.Id);
		}

	}
}

