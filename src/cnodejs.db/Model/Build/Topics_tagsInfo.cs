using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace cnodejs.Model {

	public partial class Topics_tagsInfo {
		#region fields
		private uint? _Tags_id;
		private TagsInfo _obj_tags;
		private ulong? _Topics_id;
		private TopicsInfo _obj_topics;
		#endregion

		public Topics_tagsInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Topics_tags(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Tags_id == null ? "null" : _Tags_id.ToString(), "|",
				_Topics_id == null ? "null" : _Topics_id.ToString());
		}
		public Topics_tagsInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，Topics_tagsInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Tags_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Topics_id = ulong.Parse(ret[1]);
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Tags_id") ? string.Empty : string.Format(", Tags_id : {0}", Tags_id == null ? "null" : Tags_id.ToString()), 
				__jsonIgnore.ContainsKey("Topics_id") ? string.Empty : string.Format(", Topics_id : {0}", Topics_id == null ? "null" : Topics_id.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Tags_id")) ht["Tags_id"] = Tags_id;
			if (!__jsonIgnore.ContainsKey("Topics_id")) ht["Topics_id"] = Topics_id;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(Topics_tagsInfo).GetField("JsonIgnore");
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
			Topics_tagsInfo item = obj as Topics_tagsInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(Topics_tagsInfo op1, Topics_tagsInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(Topics_tagsInfo op1, Topics_tagsInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public uint? Tags_id {
			get { return _Tags_id; }
			set {
				if (_Tags_id != value) _obj_tags = null;
				_Tags_id = value;
			}
		}
		public TagsInfo Obj_tags {
			get {
				if (_obj_tags == null) _obj_tags = cnodejs.BLL.Tags.GetItem(_Tags_id);
				return _obj_tags;
			}
			internal set { _obj_tags = value; }
		}
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
		#endregion

		public cnodejs.DAL.Topics_tags.SqlUpdateBuild UpdateDiy {
			get { return cnodejs.BLL.Topics_tags.UpdateDiy(this, _Tags_id, _Topics_id); }
		}
	}
}

