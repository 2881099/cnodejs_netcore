using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace cnodejs.Model {

	public partial class TagsInfo {
		#region fields
		private uint? _Id;
		private DateTime? _Create_time;
		private string _Keyname;
		private string _Name;
		#endregion

		public TagsInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Tags(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Keyname == null ? "null" : _Keyname.Replace("|", StringifySplit), "|",
				_Name == null ? "null" : _Name.Replace("|", StringifySplit));
		}
		public TagsInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 4, StringSplitOptions.None);
			if (ret.Length != 4) throw new Exception("格式不正确，TagsInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Create_time = new DateTime(long.Parse(ret[1]));
			if (string.Compare("null", ret[2]) != 0) _Keyname = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) _Name = ret[3].Replace(StringifySplit, "|");
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
				__jsonIgnore.ContainsKey("Keyname") ? string.Empty : string.Format(", Keyname : {0}", Keyname == null ? "null" : string.Format("'{0}'", Keyname.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Name") ? string.Empty : string.Format(", Name : {0}", Name == null ? "null" : string.Format("'{0}'", Name.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (!__jsonIgnore.ContainsKey("Keyname")) ht["Keyname"] = Keyname;
			if (!__jsonIgnore.ContainsKey("Name")) ht["Name"] = Name;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(TagsInfo).GetField("JsonIgnore");
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
			TagsInfo item = obj as TagsInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(TagsInfo op1, TagsInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(TagsInfo op1, TagsInfo op2) {
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
		public string Keyname {
			get { return _Keyname; }
			set { _Keyname = value; }
		}
		/// <summary>
		/// 标签
		/// </summary>
		public string Name {
			get { return _Name; }
			set { _Name = value; }
		}
		private List<TopicsInfo> _obj_topicss;
		public List<TopicsInfo> Obj_topicss {
			get {
				if (_obj_topicss == null) _obj_topicss = cnodejs.BLL.Topics.SelectByTags_id(_Id.Value).ToList();
				return _obj_topicss;
			}
		}
		#endregion

		public cnodejs.DAL.Tags.SqlUpdateBuild UpdateDiy {
			get { return cnodejs.BLL.Tags.UpdateDiy(this, _Id); }
		}
		public Topics_tagsInfo FlagTopics(TopicsInfo Topics) {
			return FlagTopics(Topics.Id);
		}
		public Topics_tagsInfo FlagTopics(ulong? Topics_id) {
			Topics_tagsInfo item = cnodejs.BLL.Topics_tags.GetItem(this.Id, Topics_id);
			if (item == null) item = cnodejs.BLL.Topics_tags.Insert(new Topics_tagsInfo {
				Tags_id = this.Id, 
				Topics_id = Topics_id});
			return item;
		}

		public int UnflagTopics(TopicsInfo Topics) {
			return UnflagTopics(Topics.Id);
		}
		public int UnflagTopics(ulong? Topics_id) {
			return cnodejs.BLL.Topics_tags.Delete(this.Id, Topics_id);
		}
		public int UnflagTopicsALL() {
			return cnodejs.BLL.Topics_tags.DeleteByTags_id(this.Id);
		}

	}
}

