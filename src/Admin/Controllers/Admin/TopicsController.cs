using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using cnodejs.BLL;
using cnodejs.Model;

namespace cnodejs.Admin.Controllers {
	[Route("api/[controller]")]
	public class TopicsController : BaseController {
		private readonly ILogger<TopicsController> _logger;
		public TopicsController(ILogger<TopicsController> logger) { _logger = logger; }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] ulong?[] Last_posts_id, [FromQuery] ulong?[] Owner_users_id, [FromQuery] uint[] Tags_id, [FromQuery] ulong[] Users_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Topics.Select
				.Where(!string.IsNullOrEmpty(key), "a.title like {0}", string.Concat("%", key, "%"));
			if (Last_posts_id.Length > 0) select.WhereLast_posts_id(Last_posts_id);
			if (Owner_users_id.Length > 0) select.WhereOwner_users_id(Owner_users_id);
			if (Tags_id.Length > 0) select.WhereTags_id(Tags_id);
			if (Users_id.Length > 0) select.WhereUsers_id(Users_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Posts>("b", "b.id = a.last_posts_id")
				.InnerJoin<Users>("c", "c.id = a.owner_users_id").Skip(skip).Limit(limit).ToList();
			return new APIReturn(0, "成功", "items", items.ToBson(), "count", count, 
				"items_posts", items.Select<TopicsInfo, PostsInfo>(a => a.Obj_last_posts).ToBson(), 
				"items_users", items.Select<TopicsInfo, UsersInfo>(a => a.Obj_owner_users).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(ulong? Id) {
			TopicsInfo item = Topics.GetItem(Id);
			if (item == null) return new APIReturn(98, "记录不存在，或者没有权限");
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] ulong? Last_posts_id, [FromForm] ulong? Owner_users_id, [FromForm] int? Count_posts, [FromForm] uint? Count_views, [FromForm] string Title, [FromForm] ulong? Top, [FromForm] uint[] mn_Tags, [FromForm] ulong[] mn_Users) {
			TopicsInfo item = new TopicsInfo();
			item.Last_posts_id = Last_posts_id;
			item.Owner_users_id = Owner_users_id;
			item.Count_posts = Count_posts;
			item.Count_views = Count_views;
			item.Create_time = DateTime.Now;
			item.Title = Title;
			item.Top = Top;
			item.Update_time = DateTime.Now;
			item = Topics.Insert(item);
			//关联 Tags
			foreach (uint mn_Tags_in in mn_Tags)
				item.FlagTags(mn_Tags_in);
			//关联 Users
			foreach (ulong mn_Users_in in mn_Users)
				item.FlagUsers(mn_Users_in);
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(ulong? Id, [FromForm] ulong? Last_posts_id, [FromForm] ulong? Owner_users_id, [FromForm] int? Count_posts, [FromForm] uint? Count_views, [FromForm] string Title, [FromForm] ulong? Top, [FromForm] uint[] mn_Tags, [FromForm] ulong[] mn_Users) {
			TopicsInfo item = new TopicsInfo();
			item.Id = Id;
			item.Last_posts_id = Last_posts_id;
			item.Owner_users_id = Owner_users_id;
			item.Count_posts = Count_posts;
			item.Count_views = Count_views;
			item.Create_time = DateTime.Now;
			item.Title = Title;
			item.Top = Top;
			item.Update_time = DateTime.Now;
			int affrows = Topics.Update(item);
			//关联 Tags
			if (mn_Tags.Length == 0) {
				item.UnflagTagsALL();
			} else {
				List<uint> mn_Tags_list = mn_Tags.ToList();
				foreach (TagsInfo Obj_tags in item.Obj_tagss) {
					int idx = mn_Tags_list.FindIndex(a => a == Obj_tags.Id);
					if (idx == -1) item.UnflagTags(Obj_tags.Id);
					else mn_Tags_list.RemoveAt(idx);
				}
				mn_Tags_list.ForEach(a => item.FlagTags(a));
			}
			//关联 Users
			if (mn_Users.Length == 0) {
				item.UnflagUsersALL();
			} else {
				List<ulong> mn_Users_list = mn_Users.ToList();
				foreach (UsersInfo Obj_users in item.Obj_userss) {
					int idx = mn_Users_list.FindIndex(a => a == Obj_users.Id);
					if (idx == -1) item.UnflagUsers(Obj_users.Id);
					else mn_Users_list.RemoveAt(idx);
				}
				mn_Users_list.ForEach(a => item.FlagUsers(a));
			}
			if (affrows > 0) return new APIReturn(0, "成功");
			return new APIReturn(99, "失败");
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(ulong? Id) {
			int affrows = Topics.Delete(Id);
			if (affrows > 0) return new APIReturn(0, string.Format("删除成功，影响行数：{0}", affrows));
			return new APIReturn(99, "失败");
		}
	}
}
