using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace cnodejs.Model {

	public partial class FriendlylinksInfo {
		#region fields
		private uint? _Id;
		private DateTime? _Create_time;
		private string _Link;
		private string _Logo;
		private uint? _Sort;
		private string _Title;
		#endregion

		public FriendlylinksInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Friendlylinks(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Link == null ? "null" : _Link.Replace("|", StringifySplit), "|",
				_Logo == null ? "null" : _Logo.Replace("|", StringifySplit), "|",
				_Sort == null ? "null" : _Sort.ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit));
		}
		public FriendlylinksInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 6, StringSplitOptions.None);
			if (ret.Length != 6) throw new Exception("格式不正确，FriendlylinksInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Create_time = new DateTime(long.Parse(ret[1]));
			if (string.Compare("null", ret[2]) != 0) _Link = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) _Logo = ret[3].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[4]) != 0) _Sort = uint.Parse(ret[4]);
			if (string.Compare("null", ret[5]) != 0) _Title = ret[5].Replace(StringifySplit, "|");
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
				__jsonIgnore.ContainsKey("Link") ? string.Empty : string.Format(", Link : {0}", Link == null ? "null" : string.Format("'{0}'", Link.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Logo") ? string.Empty : string.Format(", Logo : {0}", Logo == null ? "null" : string.Format("'{0}'", Logo.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Sort") ? string.Empty : string.Format(", Sort : {0}", Sort == null ? "null" : Sort.ToString()), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (!__jsonIgnore.ContainsKey("Link")) ht["Link"] = Link;
			if (!__jsonIgnore.ContainsKey("Logo")) ht["Logo"] = Logo;
			if (!__jsonIgnore.ContainsKey("Sort")) ht["Sort"] = Sort;
			if (!__jsonIgnore.ContainsKey("Title")) ht["Title"] = Title;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(FriendlylinksInfo).GetField("JsonIgnore");
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
			FriendlylinksInfo item = obj as FriendlylinksInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(FriendlylinksInfo op1, FriendlylinksInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(FriendlylinksInfo op1, FriendlylinksInfo op2) {
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
		/// 创建时间
		/// </summary>
		public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 链接地址
		/// </summary>
		public string Link {
			get { return _Link; }
			set { _Link = value; }
		}
		/// <summary>
		/// LOGO
		/// </summary>
		public string Logo {
			get { return _Logo; }
			set { _Logo = value; }
		}
		/// <summary>
		/// 排序
		/// </summary>
		public uint? Sort {
			get { return _Sort; }
			set { _Sort = value; }
		}
		/// <summary>
		/// 标题
		/// </summary>
		public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		#endregion

		public cnodejs.DAL.Friendlylinks.SqlUpdateBuild UpdateDiy {
			get { return cnodejs.BLL.Friendlylinks.UpdateDiy(this, _Id); }
		}
	}
}

