using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace cnodejs.Model {
	public static partial class ExtensionMethods {
		public static string ToJson(this FriendlylinksInfo item) { return string.Concat(item); }
		public static string ToJson(this FriendlylinksInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<FriendlylinksInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this FriendlylinksInfo[] items, Func<FriendlylinksInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<FriendlylinksInfo> items, Func<FriendlylinksInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this PostsInfo item) { return string.Concat(item); }
		public static string ToJson(this PostsInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<PostsInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this PostsInfo[] items, Func<PostsInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<PostsInfo> items, Func<PostsInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this SysdocInfo item) { return string.Concat(item); }
		public static string ToJson(this SysdocInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<SysdocInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this SysdocInfo[] items, Func<SysdocInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<SysdocInfo> items, Func<SysdocInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this TagsInfo item) { return string.Concat(item); }
		public static string ToJson(this TagsInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<TagsInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this TagsInfo[] items, Func<TagsInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<TagsInfo> items, Func<TagsInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this TopicsInfo item) { return string.Concat(item); }
		public static string ToJson(this TopicsInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<TopicsInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this TopicsInfo[] items, Func<TopicsInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<TopicsInfo> items, Func<TopicsInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Topics_tagsInfo item) { return string.Concat(item); }
		public static string ToJson(this Topics_tagsInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Topics_tagsInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Topics_tagsInfo[] items, Func<Topics_tagsInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Topics_tagsInfo> items, Func<Topics_tagsInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this UsersInfo item) { return string.Concat(item); }
		public static string ToJson(this UsersInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<UsersInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this UsersInfo[] items, Func<UsersInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<UsersInfo> items, Func<UsersInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Users_topicsInfo item) { return string.Concat(item); }
		public static string ToJson(this Users_topicsInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Users_topicsInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Users_topicsInfo[] items, Func<Users_topicsInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Users_topicsInfo> items, Func<Users_topicsInfo, object> func = null) { return GetBson(items, func); }

		public static string GetJson(IEnumerable items) {
			StringBuilder sb = new StringBuilder();
			sb.Append("[");
			IEnumerator ie = items.GetEnumerator();
			if (ie.MoveNext()) {
				while (true) {
					sb.Append(string.Concat(ie.Current));
					if (ie.MoveNext()) sb.Append(",");
					else break;
				}
			}
			sb.Append("]");
			return sb.ToString();
		}
		public static IDictionary[] GetBson(IEnumerable items, Delegate func = null) {
			List<IDictionary> ret = new List<IDictionary>();
			IEnumerator ie = items.GetEnumerator();
			while (ie.MoveNext()) {
				if (ie.Current == null) ret.Add(null);
				else if (func == null) ret.Add(ie.Current.GetType().GetMethod("ToBson").Invoke(ie.Current, null) as IDictionary);
				else {
					object obj = func.GetMethodInfo().Invoke(func.Target, new object[] { ie.Current });
					if (obj is IDictionary) ret.Add(obj as IDictionary);
					else {
						Hashtable ht = new Hashtable();
						PropertyInfo[] pis = obj.GetType().GetProperties();
						foreach (PropertyInfo pi in pis) ht[pi.Name] = pi.GetValue(obj);
						ret.Add(ht);
					}
				}
			}
			return ret.ToArray();
		}
	}
}